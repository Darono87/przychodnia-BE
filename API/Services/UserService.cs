// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Repositories;
using API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericUserRepository<Admin> adminRepository;
        private readonly DataContext context;
        private readonly IGenericUserRepository<Doctor> doctorRepository;
        private readonly IJwtManager jwtManager;
        private readonly IGenericUserRepository<LabManager> labManagerRepository;
        private readonly IGenericUserRepository<LabTechnician> labTechnicianRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IGenericUserRepository<Registrar> registrarRepository;

        private readonly string[] roles =
        {
            typeof(User).ToString(), typeof(Registrar).ToString(), typeof(Doctor).ToString(),
            typeof(LabTechnician).ToString(), typeof(LabManager).ToString(), typeof(Admin).ToString()
        };

        private readonly IGenericUserRepository<User> userRepository;

        public UserService(
            IGenericUserRepository<User> userRepository,
            IGenericUserRepository<Admin> adminRepository,
            IGenericUserRepository<Registrar> registrarRepository,
            IGenericUserRepository<Doctor> doctorRepository,
            IGenericUserRepository<LabTechnician> labTechnicianRepository,
            IGenericUserRepository<LabManager> labManagerRepository,
            DataContext context,
            IJwtManager jwtManager, IRefreshTokenRepository refreshTokenRepository)
        {
            this.userRepository = userRepository;
            this.adminRepository = adminRepository;
            this.registrarRepository = registrarRepository;
            this.doctorRepository = doctorRepository;
            this.labTechnicianRepository = labTechnicianRepository;
            this.labManagerRepository = labManagerRepository;
            this.context = context;
            this.jwtManager = jwtManager;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<IActionResult> GetSuggestionsAsync(string role){
            var roleAtr = new RoleAttribute();
            if(!roleAtr.IsValid(role)){
                return new JsonResult(new ExceptionDto {Message = roleAtr.ErrorMessage}) {StatusCode = 422};
            }
            SuggestionsDto suggestions; 
            switch(role){
                case "LabTechnician":
                    var techs = await labTechnicianRepository.GetAllAsync();
                    suggestions = new SuggestionsDto { Suggestions = techs.Select(tech => { 
                        return new SuggestionsDto.Suggestion{value = tech.Id, label = $"{tech.User.FirstName} {tech.User.LastName}"};
                    })};
                    break;
                case "Registrar":
                    var regs = await registrarRepository.GetAllAsync();
                    suggestions = new SuggestionsDto { Suggestions = regs.Select(reg => { 
                        return new SuggestionsDto.Suggestion{value = reg.Id, label = $"{reg.User.FirstName} {reg.User.LastName}"};
                    })};
                    break;
                case "Admin":
                    var admins = await adminRepository.GetAllAsync();
                    suggestions = new SuggestionsDto { Suggestions = admins.Select(adm => { 
                        return new SuggestionsDto.Suggestion{value = adm.Id, label = $"{adm.User.FirstName} {adm.User.LastName}"};
                    })};
                    break;
                case "Doctor":
                    var doctors = await doctorRepository.GetAllAsync();
                    suggestions = new SuggestionsDto { Suggestions = doctors.Select(dc => { 
                        return new SuggestionsDto.Suggestion{value = dc.Id, label = $"{dc.User.FirstName} {dc.User.LastName}"};
                    })};
                    break;
                case "LabManager":
                default:
                    var managers = await labManagerRepository.GetAllAsync();
                    suggestions = new SuggestionsDto { Suggestions = managers.Select(mgr => { 
                        return new SuggestionsDto.Suggestion{value = mgr.Id, label = $"{mgr.User.FirstName} {mgr.User.LastName}"};
                    })};
                    break;
            }

            return new JsonResult(suggestions);
        }

        public async Task<IActionResult> CreateAsync(string role, string login, string firstName, string lastName,
            string password,
            string? permitNumber)
        {
            var existingUser = await userRepository.GetAsync(login);

            if (existingUser != null)
            {
                return new JsonResult(new ExceptionDto {Message = "Login is already in use"}) {StatusCode = 422};
            }

            if (role == "Doctor" && string.IsNullOrEmpty(permitNumber))
            {
                return new JsonResult(new ExceptionDto {Message = "Doctor role requires a permit number"})
                {
                    StatusCode = 422
                };
            }

            context.Database?.BeginTransactionAsync();

            var createdUser = await userRepository.AddAsync(new User
            {
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            });

            switch (role)
            {
                case "Admin":
                    createdUser.Admin = await adminRepository.AddAsync(new Admin {User = createdUser});
                    break;
                case "Registrar":
                    createdUser.Registrar = await registrarRepository.AddAsync(new Registrar {User = createdUser});
                    break;
                case "Doctor":
                    createdUser.Doctor =
                        await doctorRepository.AddAsync(new Doctor {User = createdUser, PermitNumber = permitNumber});
                    break;
                case "LabTechnician":
                    createdUser.LabTechnician =
                        await labTechnicianRepository.AddAsync(new LabTechnician {User = createdUser});
                    break;
                case "LabManager":
                    createdUser.LabManager = await labManagerRepository.AddAsync(new LabManager {User = createdUser});
                    break;
            }

            context.Database?.CommitTransactionAsync();

            return new JsonResult(createdUser) {StatusCode = 201};
        }

        public async Task<IActionResult> AuthenticateAsync(string login, string password)
        {
            var user = await userRepository.GetAsync(login);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new JsonResult(new ExceptionDto {Message = "Invalid credentials"}) {StatusCode = 422};
            }

            var tokens = jwtManager.GenerateTokens(login, await GetRoleAsync(login), DateTime.Now);

            var handler = new JwtSecurityTokenHandler();
            var refreshData = handler.ReadJwtToken(tokens.RefreshToken);
            var date = refreshData.ValidTo;

            await refreshTokenRepository.Add(new RefreshToken {Token = tokens.RefreshToken, ValidTill = date});

            return new JsonResult(tokens);
        }

        public async Task<IActionResult> RefreshAsync(string accessToken, string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(accessToken);
            var refreshData = handler.ReadJwtToken(refreshToken);
            var date = refreshData.ValidTo;

            if (DateTime.Now > date)
            {
                return new JsonResult(new ExceptionDto {Message = "Refresh Token is expired"}) {StatusCode = 422};
            }

            var refreshTokenEntity = await refreshTokenRepository.Get(refreshToken);

            var login = data.Claims.Where(c => c.Type == ClaimTypes.Name).ToArray()[0].Value;
            var user = await userRepository.GetAsync(login);

            if (user == null || refreshTokenEntity == null)
            {
                return new JsonResult(new ExceptionDto {Message = "User does not exist or refresh token is invalid"})
                {
                    StatusCode = 422
                };
            }

            await refreshTokenRepository.Remove(refreshToken);

            var tokens = jwtManager.GenerateTokens(user.Login, await GetRoleAsync(user.Login), DateTime.Now);

            refreshData = handler.ReadJwtToken(tokens.RefreshToken);
            date = refreshData.ValidTo;

            await refreshTokenRepository.Add(new RefreshToken {Token = tokens.RefreshToken, ValidTill = date});

            return new JsonResult(jwtManager.GenerateTokens(user.Login, await GetRoleAsync(user.Login), DateTime.Now));
        }

        public async Task<string> GetRoleAsync(string login)
        {
            if (login == "")
            {
                throw new ArgumentException("Login cannot be empty");
            }

            var user = await userRepository.GetAsync(login);

            if (user == null)
            {
                throw new ArgumentException("User with given login does not exist");
            }

            string role = "";

            role += await adminRepository.GetAsync(login) != null ? "Admin" : "";
            role += await registrarRepository.GetAsync(login) != null ? "Registrar" : "";
            role += await doctorRepository.GetAsync(login) != null ? "Doctor" : "";
            role += await labTechnicianRepository.GetAsync(login) != null ? "LabTechnician" : "";
            role += await labManagerRepository.GetAsync(login) != null ? "LabManager" : "";

            return role;
        }

        public async Task<object> GetCurrentUserAsync(HttpRequest request)
        {
            var accessToken = request.Headers[HeaderNames.Authorization][0].Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(accessToken);

            var login = data.Claims.Where(c => c.Type == ClaimTypes.Name).ToArray()[0].Value;
            var role = data.Claims.Where(c => c.Type == ClaimTypes.Role).ToArray()[0].Value;

            return role switch
            {
                "Admin" => await adminRepository.GetAsync(login),
                "Registrar" => await registrarRepository.GetAsync(login),
                "Doctor" => await doctorRepository.GetAsync(login),
                "LabTechnician" => await labTechnicianRepository.GetAsync(login),
                "LabManager" => await labManagerRepository.GetAsync(login),
                _ => throw new ArgumentOutOfRangeException("request", "Does not contain valid token")
            };
        }
    }
}
