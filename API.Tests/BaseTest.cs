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
        protected Mock<IGenericUserRepository<User>> mockUserRepository;
        protected IGenericUserRepository<User> userRepository;
        protected Mock<IGenericUserRepository<Admin>> mockAdminRepository;
        protected IGenericUserRepository<Admin> adminRepository;
        protected Mock<IGenericUserRepository<Registrator>> mockRegistratorRepository;
        protected IGenericUserRepository<Registrator> registratorRepository;
        protected Mock<IGenericUserRepository<Doctor>> mockDoctorRepository;
        protected IGenericUserRepository<Doctor> doctorRepository;
        protected Mock<IGenericUserRepository<LabTechnician>> mockLabTechnicianRepository;
        protected IGenericUserRepository<LabTechnician> labTechnicianRepository;
        protected Mock<IGenericUserRepository<LabManager>> mockLabManagerRepository;
        protected IGenericUserRepository<LabManager> labManagerRepository;
        protected Mock<IJwtManager> mockJwtManager;
        protected IJwtManager jwtManager;
        
        protected IUserService userService;

        private void Setup()
        {
            mockUserRepository = new Mock<IGenericUserRepository<User>>();
            mockAdminRepository = new Mock<IGenericUserRepository<Admin>>();
            mockRegistratorRepository = new Mock<IGenericUserRepository<Registrator>>();
            mockDoctorRepository = new Mock<IGenericUserRepository<Doctor>>();
            mockLabTechnicianRepository = new Mock<IGenericUserRepository<LabTechnician>>();
            mockLabManagerRepository = new Mock<IGenericUserRepository<LabManager>>();
            mockJwtManager = new Mock<IJwtManager>();
            
            // mock user repository setup
            // password hash everywhere is for 'Qwerty1234'
            mockUserRepository.Setup(o => o.Get(It.IsIn(1)))
                .Returns(new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." });
            mockUserRepository.Setup(o => o.Get(It.IsIn(2)))
                .Returns(new User() {Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." });
            mockUserRepository.Setup(o => o.Get(It.IsIn("user1")))
                .Returns(new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." });
            mockUserRepository.Setup(o => o.Get(It.IsIn("user2")))
                .Returns(new User() {Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." });
            mockUserRepository.Setup(o => o.Add(It.IsIn<User>(null)))
                .Throws(new ArgumentException());
            mockUserRepository.Setup(o => o.Add(It.IsNotNull<User>()));
            mockUserRepository.Setup(o => o.Update(It.IsAny<User>()));

            // mock admin repository setup
            mockAdminRepository.Setup(o => o.Get(It.IsIn(1)))
                .Returns(new Admin()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockAdminRepository.Setup(o => o.Get(It.IsIn(2)))
                .Returns(new Admin()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockAdminRepository.Setup(o => o.Get(It.IsIn("user1")))
                .Returns(new Admin()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockAdminRepository.Setup(o => o.Get(It.IsIn("user2")))
                .Returns(new Admin()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockAdminRepository.Setup(o => o.Add(It.IsIn<Admin>(null)))
                .Throws(new ArgumentException());
            mockAdminRepository.Setup(o => o.Add(It.IsNotNull<Admin>()));
            mockAdminRepository.Setup(o => o.Update(It.IsAny<Admin>()));
            
            // mock registrator repository setup
            mockRegistratorRepository.Setup(o => o.Get(It.IsIn(1)))
                .Returns(new Registrator()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockRegistratorRepository.Setup(o => o.Get(It.IsIn(2)))
                .Returns(new Registrator()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockRegistratorRepository.Setup(o => o.Get(It.IsIn("user1")))
                .Returns(new Registrator()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockRegistratorRepository.Setup(o => o.Get(It.IsIn("user2")))
                .Returns(new Registrator()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockRegistratorRepository.Setup(o => o.Add(It.IsIn<Registrator>(null)))
                .Throws(new ArgumentException());
            mockRegistratorRepository.Setup(o => o.Add(It.IsNotNull<Registrator>()));
            mockRegistratorRepository.Setup(o => o.Update(It.IsAny<Registrator>()));
            
            // mock doctor repository setup
            mockDoctorRepository.Setup(o => o.Get(It.IsIn(1)))
                .Returns(new Doctor()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockDoctorRepository.Setup(o => o.Get(It.IsIn(2)))
                .Returns(new Doctor()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockDoctorRepository.Setup(o => o.Get(It.IsIn("user1")))
                .Returns(new Doctor()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockDoctorRepository.Setup(o => o.Get(It.IsIn("user2")))
                .Returns(new Doctor()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockDoctorRepository.Setup(o => o.Add(It.IsIn<Doctor>(null)))
                .Throws(new ArgumentException());
            mockDoctorRepository.Setup(o => o.Add(It.IsNotNull<Doctor>()));
            mockDoctorRepository.Setup(o => o.Update(It.IsAny<Doctor>()));
            
            // mock lab technician repository setup
            mockLabTechnicianRepository.Setup(o => o.Get(It.IsIn(1)))
                .Returns(new LabTechnician()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockLabTechnicianRepository.Setup(o => o.Get(It.IsIn(2)))
                .Returns(new LabTechnician()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockLabTechnicianRepository.Setup(o => o.Get(It.IsIn("user1")))
                .Returns(new LabTechnician()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockLabTechnicianRepository.Setup(o => o.Get(It.IsIn("user2")))
                .Returns(new LabTechnician()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockLabTechnicianRepository.Setup(o => o.Add(It.IsIn<LabTechnician>(null)))
                .Throws(new ArgumentException());
            mockLabTechnicianRepository.Setup(o => o.Add(It.IsNotNull<LabTechnician>()));
            mockLabTechnicianRepository.Setup(o => o.Update(It.IsAny<LabTechnician>()));

            // mock lab manager repository setup
            mockLabManagerRepository.Setup(o => o.Get(It.IsIn(1)))
                .Returns(new LabManager()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockLabManagerRepository.Setup(o => o.Get(It.IsIn(2)))
                .Returns(new LabManager()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockLabManagerRepository.Setup(o => o.Get(It.IsIn("user1")))
                .Returns(new LabManager()
                {
                    Id = 1, User = new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockLabManagerRepository.Setup(o => o.Get(It.IsIn("user2")))
                .Returns(new LabManager()
                {
                    Id = 2, User = new User() { Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2b$10$vpOJvQlBeI.BFMpeiJ9Jq.f//JeoAqRXKRXt5p.swv.Qj9L9nBq4." }
                });
            mockLabManagerRepository.Setup(o => o.Add(It.IsIn<LabManager>(null)))
                .Throws(new ArgumentException());
            mockLabManagerRepository.Setup(o => o.Add(It.IsNotNull<LabManager>()));
            mockLabManagerRepository.Setup(o => o.Update(It.IsAny<LabManager>()));

            mockJwtManager.Setup(o => o.GenerateTokens(It.IsIn("user1"), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(new AuthenticationDTO()
                {
                    AccessToken = "access_token1", RefreshToken = "refresh_token1", Role = "User"
                });
            mockJwtManager.Setup(o => o.GenerateTokens(It.IsIn("user2"), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(new AuthenticationDTO()
                {
                    AccessToken = "access_token2", RefreshToken = "refresh_token2", Role = "User"
                });
            mockJwtManager.Setup(o => o.GenerateTokens(It.IsIn(""), It.IsIn(""), It.IsAny<DateTime>()))
                .Throws<ArgumentException>();

            userRepository = mockUserRepository.Object;
            adminRepository = mockAdminRepository.Object;
            registratorRepository = mockRegistratorRepository.Object;
            doctorRepository = mockDoctorRepository.Object;
            labTechnicianRepository = mockLabTechnicianRepository.Object;
            labManagerRepository = mockLabManagerRepository.Object;
            jwtManager = mockJwtManager.Object;
        }

        public BaseTest()
        {
            Setup();

            var dataContext = new Mock<DataContext>(new DbContextOptionsBuilder().Options);

            userService = new UserService(
                userRepository, 
                adminRepository, 
                registratorRepository,
                doctorRepository, 
                labTechnicianRepository, 
                labManagerRepository,
                dataContext.Object,
                jwtManager
            );
        }
    }
}
