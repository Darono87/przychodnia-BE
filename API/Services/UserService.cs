﻿// Licensed to the .NET Foundation under one or more agreements.
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
using API.Exceptions;
using API.Repositories;
using API.Utils;
using Microsoft.AspNetCore.Http;
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
            IJwtManager jwtManager)
        {
            this.userRepository = userRepository;
            this.adminRepository = adminRepository;
            this.registrarRepository = registrarRepository;
            this.doctorRepository = doctorRepository;
            this.labTechnicianRepository = labTechnicianRepository;
            this.labManagerRepository = labManagerRepository;
            this.context = context;
            this.jwtManager = jwtManager;
        }

        public async Task<User> Create(string role, string login, string firstName, string lastName, string password,
            string? permitNumber)
        {
            if (login == "" || firstName == "" || lastName == "" || password == "")
            {
                throw new ArgumentException("User properties cannot be empty");
            }

            var existingUser = userRepository.GetAsync(login);

            if (existingUser != null)
            {
                throw new LoginTakenException();
            }

            if (!roles.Any(r => r.ToLower().Contains(role.ToLower())))
            {
                throw new RoleNotFoundException();
            }

            if (!Helper.ValidatePassword(password))
            {
                throw new InvalidPasswordException();
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
                    createdUser.Doctor = await doctorRepository.AddAsync(new Doctor {User = createdUser, PermitNumber = permitNumber});
                    break;
                case "LabTechnician":
                    createdUser.LabTechnician = await labTechnicianRepository.AddAsync(new LabTechnician {User = createdUser});
                    break;
                case "LabManager":
                    createdUser.LabManager = await labManagerRepository.AddAsync(new LabManager {User = createdUser});
                    break;
            }

            context.Database?.CommitTransactionAsync();

            return createdUser;
        }

        public async Task<AuthenticationDTO> Authenticate(string login, string password)
        {
            if (login.Trim() == "" || password.Trim() == "")
            {
                throw new ArgumentException("Login / password cannot be empty");
            }

            var user = await userRepository.GetAsync(login);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new ArgumentException("Invalid credentials");
            }

            return jwtManager.GenerateTokens(login, await GetRole(login), DateTime.Now);
        }

        public async Task<AuthenticationDTO> Refresh(string accessToken, string refreshToken)
        {
            if (accessToken.Trim() == "" || refreshToken.Trim() == "")
            {
                throw new ArgumentException("Tokens cannot be empty");
            }

            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(accessToken);
            var refreshData = handler.ReadJwtToken(refreshToken);
            var date = refreshData.ValidTo;

            if (DateTime.Now > date)
            {
                throw new ArgumentException("Refresh token is expired");
            }

            var login = data.Claims.Where(c => c.Type == ClaimTypes.Name).ToArray()[0].Value;
            var user = await userRepository.GetAsync(login);

            if (user == null || !jwtManager.ContainsRefreshToken(refreshToken))
            {
                throw new ArgumentException("User does not exist or refresh token is invalid");
            }

            return jwtManager.GenerateTokens(user.Login, await GetRole(user.Login), DateTime.Now);
        }

        public async Task<string> GetRole(string login)
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

            role += (await adminRepository.GetAsync(login)) != null ? "Admin" : "";
            role += (await registrarRepository.GetAsync(login)) != null ? "Registrar" : "";
            role += (await doctorRepository.GetAsync(login)) != null ? "Doctor" : "";
            role += (await labTechnicianRepository.GetAsync(login)) != null ? "LabTechnician" : "";
            role += (await labManagerRepository.GetAsync(login)) != null ? "LabManager" : "";

            return role;
        }

        public async Task<object> GetCurrentUser(HttpRequest request)
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
