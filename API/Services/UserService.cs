// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly DataContext context;
        private readonly IGenericUserRepository<User> userRepository;
        private readonly IGenericUserRepository<Admin> adminRepository;
        private readonly IGenericUserRepository<Registrator> registratorRepository;
        private readonly IGenericUserRepository<Doctor> doctorRepository;
        private readonly IGenericUserRepository<LabTechnician> labTechnicianRepository;
        private readonly IGenericUserRepository<LabManager> labManagerRepository;
        private readonly IJwtManager jwtManager;
        
        private readonly string[] roles = 
        {
            typeof(User).ToString(), typeof(Registrator).ToString(), typeof(Doctor).ToString(),
            typeof(LabTechnician).ToString(), typeof(LabManager).ToString(), typeof(Admin).ToString()
        };

        public UserService(
            IGenericUserRepository<User> userRepository, 
            IGenericUserRepository<Admin> adminRepository,
            IGenericUserRepository<Registrator> registratorRepository,
            IGenericUserRepository<Doctor> doctorRepository,
            IGenericUserRepository<LabTechnician> labTechnicianRepository,
            IGenericUserRepository<LabManager> labManagerRepository,
            DataContext context,
            IJwtManager jwtManager)
        {
            this.userRepository = userRepository;
            this.adminRepository = adminRepository;
            this.registratorRepository = registratorRepository;
            this.doctorRepository = doctorRepository;
            this.labTechnicianRepository = labTechnicianRepository;
            this.labManagerRepository = labManagerRepository;
            this.context = context;
            this.jwtManager = jwtManager;
        }

        public void Create(string role, string login, string firstName, string lastName, string password, string? permitNumber)
        {
            if (login == "" || firstName == "" || lastName == "" || password == "")
            {
                throw new ArgumentException("User properties cannot be empty");
            }

            var existingUser = userRepository.Get(login);

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

            context.Database?.BeginTransaction();
            
            userRepository.Add(new User
            {
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            });

            var createdUser = userRepository.Get(login);

            switch (role)
            {
                case "Admin":
                    adminRepository.Add(new Admin()
                    {
                        User = createdUser
                    });
                    break;
                case "Registrator":
                    registratorRepository.Add(new Registrator()
                    {
                        User = createdUser
                    });                    
                    break;
                case "Doctor":
                    doctorRepository.Add(new Doctor()
                    {
                        User = createdUser,
                        PermitNumber = permitNumber
                    });
                    break;
                case "LabTechnician":
                    labTechnicianRepository.Add(new LabTechnician()
                    {
                        User = createdUser
                    });
                    break;
                case "LabManager":
                    labManagerRepository.Add(new LabManager()
                    {
                        User = createdUser
                    });                    
                    break;
            }
            
            context.Database?.CommitTransaction();
        }

        public AuthenticationDTO Authenticate(string login, string password)
        {
            if (login.Trim() == "" || password.Trim() == "")
            {
                throw new ArgumentException("Login / password cannot be empty");
            }

            var user = userRepository.Get(login);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new ArgumentException("Invalid credentials");
            }

            return jwtManager.GenerateTokens(login, GetRole(login), DateTime.Now);
        }

        public string GetRole(string login)
        {
            if (login == "")
            {
                throw new ArgumentException("Login cannot be empty");
            }

            var user = userRepository.Get(login);

            if (user == null)
            {
                throw new ArgumentException("User with given login does not exist");
            }

            string role = "";

            role += adminRepository.Get(login) != null ? "Admin" : "";
            role += registratorRepository.Get(login) != null ? "Registrator" : "";
            role += doctorRepository.Get(login) != null ? "Doctor" : "";
            role += labTechnicianRepository.Get(login) != null ? "LabTechnician" : "";
            role += labManagerRepository.Get(login) != null ? "LabManager" : "";

            return role;
        }
        
        public object GetCurrentUser(HttpRequest request)
        {
            var accessToken = request.Headers[HeaderNames.Authorization][0].Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(accessToken);
            
            var login = data.Claims.Where(c => c.Type == ClaimTypes.Name).ToArray()[0].Value;
            var role = data.Claims.Where(c => c.Type == ClaimTypes.Role).ToArray()[0].Value;

            return role switch
            {
                "Admin" => adminRepository.Get(login),
                "Registrator" => registratorRepository.Get(login),
                "Doctor" => doctorRepository.Get(login),
                "LabTechnician" => labTechnicianRepository.Get(login),
                "LabManager" => labManagerRepository.Get(login),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
