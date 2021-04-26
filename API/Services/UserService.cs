// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using System.Linq;
using API.Data;
using API.Entities;
using API.Exceptions;
using API.Repositories;
using API.Utils;

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
            DataContext context)
        {
            this.userRepository = userRepository;
            this.adminRepository = adminRepository;
            this.registratorRepository = registratorRepository;
            this.doctorRepository = doctorRepository;
            this.labTechnicianRepository = labTechnicianRepository;
            this.labManagerRepository = labManagerRepository;
            this.context = context;
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
                        User = createdUser
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

        public void Authenticate(string login, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
