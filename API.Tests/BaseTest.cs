// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using API.Data;
using API.DTO;
using API.Entities;
using API.Repositories;
using API.Services;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace API.Tests
{
    public class BaseTest
    {
        protected IGenericUserRepository<Admin> adminRepository;
        protected IGenericUserRepository<Doctor> doctorRepository;
        protected IJwtManager jwtManager;
        protected IGenericUserRepository<LabManager> labManagerRepository;
        protected IGenericUserRepository<LabTechnician> labTechnicianRepository;
        protected Mock<IGenericUserRepository<Admin>> mockAdminRepository;
        protected Mock<IGenericUserRepository<Doctor>> mockDoctorRepository;
        protected Mock<IJwtManager> mockJwtManager;
        protected Mock<IGenericUserRepository<LabManager>> mockLabManagerRepository;
        protected Mock<IGenericUserRepository<LabTechnician>> mockLabTechnicianRepository;
        protected Mock<IGenericUserRepository<Registrar>> mockRegistrarRepository;
        protected Mock<IGenericUserRepository<User>> mockUserRepository;
        protected IGenericUserRepository<Registrar> registrarRepository;
        protected IGenericUserRepository<User> userRepository;

        protected IUserService userService;

        public BaseTest()
        {
            Setup();

            var dataContext = new Mock<DataContext>(new DbContextOptionsBuilder().Options);

            userService = new UserService(
                userRepository,
                adminRepository,
                registrarRepository,
                doctorRepository,
                labTechnicianRepository,
                labManagerRepository,
                dataContext.Object,
                jwtManager
            );
        }

        private void Setup()
        {
            mockUserRepository = new Mock<IGenericUserRepository<User>>();
            mockAdminRepository = new Mock<IGenericUserRepository<Admin>>();
            mockRegistrarRepository = new Mock<IGenericUserRepository<Registrar>>();
            mockDoctorRepository = new Mock<IGenericUserRepository<Doctor>>();
            mockLabTechnicianRepository = new Mock<IGenericUserRepository<LabTechnician>>();
            mockLabManagerRepository = new Mock<IGenericUserRepository<LabManager>>();
            mockJwtManager = new Mock<IJwtManager>();

            // mock user repository setup
            // password hash everywhere is for 'Qwerty1234'
            mockUserRepository.Setup(o => o.GetAsync(It.IsIn(1)))
                .Returns(new User
                {
                    Id = 1,
                    Login = "user1",
                    FirstName = "John",
                    LastName = "Doe",
                    PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                });
            mockUserRepository.Setup(o => o.GetAsync(It.IsIn(2)))
                .Returns(new User
                {
                    Id = 2,
                    Login = "user2",
                    FirstName = "Jane",
                    LastName = "Smith",
                    PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                });
            mockUserRepository.Setup(o => o.GetAsync(It.IsIn("user1")))
                .Returns(new User
                {
                    Id = 1,
                    Login = "user1",
                    FirstName = "John",
                    LastName = "Doe",
                    PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                });
            mockUserRepository.Setup(o => o.GetAsync(It.IsIn("user2")))
                .Returns(new User
                {
                    Id = 2,
                    Login = "user2",
                    FirstName = "Jane",
                    LastName = "Smith",
                    PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                });
            mockUserRepository.Setup(o => o.AddAsync(It.IsIn<User>(null)))
                .Throws(new ArgumentException());
            mockUserRepository.Setup(o => o.AddAsync(It.IsNotNull<User>()));
            mockUserRepository.Setup(o => o.UpdateAsync(It.IsAny<User>()));

            // mock admin repository setup
            mockAdminRepository.Setup(o => o.GetAsync(It.IsIn(1)))
                .Returns(new Admin
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockAdminRepository.Setup(o => o.GetAsync(It.IsIn(2)))
                .Returns(new Admin
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockAdminRepository.Setup(o => o.GetAsync(It.IsIn("user1")))
                .Returns(new Admin
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockAdminRepository.Setup(o => o.GetAsync(It.IsIn("user2")))
                .Returns(new Admin
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockAdminRepository.Setup(o => o.AddAsync(It.IsIn<Admin>(null)))
                .Throws(new ArgumentException());
            mockAdminRepository.Setup(o => o.AddAsync(It.IsNotNull<Admin>()));
            mockAdminRepository.Setup(o => o.UpdateAsync(It.IsAny<Admin>()));

            // mock registrar repository setup
            mockRegistrarRepository.Setup(o => o.GetAsync(It.IsIn(1)))
                .Returns(new Registrar
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockRegistrarRepository.Setup(o => o.GetAsync(It.IsIn(2)))
                .Returns(new Registrar
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockRegistrarRepository.Setup(o => o.GetAsync(It.IsIn("user1")))
                .Returns(new Registrar
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockRegistrarRepository.Setup(o => o.GetAsync(It.IsIn("user2")))
                .Returns(new Registrar
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockRegistrarRepository.Setup(o => o.AddAsync(It.IsIn<Registrar>(null)))
                .Throws(new ArgumentException());
            mockRegistrarRepository.Setup(o => o.AddAsync(It.IsNotNull<Registrar>()));
            mockRegistrarRepository.Setup(o => o.UpdateAsync(It.IsAny<Registrar>()));

            // mock doctor repository setup
            mockDoctorRepository.Setup(o => o.GetAsync(It.IsIn(1)))
                .Returns(new Doctor
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockDoctorRepository.Setup(o => o.GetAsync(It.IsIn(2)))
                .Returns(new Doctor
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockDoctorRepository.Setup(o => o.GetAsync(It.IsIn("user1")))
                .Returns(new Doctor
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockDoctorRepository.Setup(o => o.GetAsync(It.IsIn("user2")))
                .Returns(new Doctor
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockDoctorRepository.Setup(o => o.AddAsync(It.IsIn<Doctor>(null)))
                .Throws(new ArgumentException());
            mockDoctorRepository.Setup(o => o.AddAsync(It.IsNotNull<Doctor>()));
            mockDoctorRepository.Setup(o => o.UpdateAsync(It.IsAny<Doctor>()));

            // mock lab technician repository setup
            mockLabTechnicianRepository.Setup(o => o.GetAsync(It.IsIn(1)))
                .Returns(new LabTechnician
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockLabTechnicianRepository.Setup(o => o.GetAsync(It.IsIn(2)))
                .Returns(new LabTechnician
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockLabTechnicianRepository.Setup(o => o.GetAsync(It.IsIn("user1")))
                .Returns(new LabTechnician
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockLabTechnicianRepository.Setup(o => o.GetAsync(It.IsIn("user2")))
                .Returns(new LabTechnician
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockLabTechnicianRepository.Setup(o => o.AddAsync(It.IsIn<LabTechnician>(null)))
                .Throws(new ArgumentException());
            mockLabTechnicianRepository.Setup(o => o.AddAsync(It.IsNotNull<LabTechnician>()));
            mockLabTechnicianRepository.Setup(o => o.UpdateAsync(It.IsAny<LabTechnician>()));

            // mock lab manager repository setup
            mockLabManagerRepository.Setup(o => o.GetAsync(It.IsIn(1)))
                .Returns(new LabManager
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockLabManagerRepository.Setup(o => o.GetAsync(It.IsIn(2)))
                .Returns(new LabManager
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockLabManagerRepository.Setup(o => o.GetAsync(It.IsIn("user1")))
                .Returns(new LabManager
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Login = "user1",
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockLabManagerRepository.Setup(o => o.GetAsync(It.IsIn("user2")))
                .Returns(new LabManager
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Login = "user2",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4."
                    }
                });
            mockLabManagerRepository.Setup(o => o.AddAsync(It.IsIn<LabManager>(null)))
                .Throws(new ArgumentException());
            mockLabManagerRepository.Setup(o => o.AddAsync(It.IsNotNull<LabManager>()));
            mockLabManagerRepository.Setup(o => o.UpdateAsync(It.IsAny<LabManager>()));

            mockJwtManager.Setup(o => o.GenerateTokens(It.IsIn("user1"), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(new AuthenticationDto
                {
                    AccessToken = "access_token1", RefreshToken = "refresh_token1", Role = "User"
                });
            mockJwtManager.Setup(o => o.GenerateTokens(It.IsIn("user2"), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(new AuthenticationDto
                {
                    AccessToken = "access_token2", RefreshToken = "refresh_token2", Role = "User"
                });
            mockJwtManager.Setup(o => o.GenerateTokens(It.IsIn(""), It.IsIn(""), It.IsAny<DateTime>()))
                .Throws<ArgumentException>();
            mockJwtManager.Setup(o => o.ContainsRefreshToken("refresh_token1"))
                .Returns(true);

            userRepository = mockUserRepository.Object;
            adminRepository = mockAdminRepository.Object;
            registrarRepository = mockRegistrarRepository.Object;
            doctorRepository = mockDoctorRepository.Object;
            labTechnicianRepository = mockLabTechnicianRepository.Object;
            labManagerRepository = mockLabManagerRepository.Object;
            jwtManager = mockJwtManager.Object;
        }
    }
}
