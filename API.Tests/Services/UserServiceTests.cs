﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using API.Entities;
using API.Exceptions;
using API.Repositories;
using API.Services;
using Moq;
using Xunit;

namespace API.Tests.Services
{
    public class UserServiceTests : BaseTest
    {
        [Fact]
        public void TestCreateWithEmptyStrings()
        {
            Assert.Throws<ArgumentException>(() => 
                userService.Create("", "", "", "", "", null));
            
            mockUserRepository.Verify(o => o.Get(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(o => o.Add(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestCreateWithTakenLogin()
        {
            Assert.Throws<LoginTakenException>(() => 
                userService.Create("User", "user1", "abc", "abc", "abc", null));
            
            mockUserRepository.Verify(o => o.Get(It.IsAny<string>()), Times.Once);
            mockUserRepository.Verify(o => o.Add(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestCreateWithInvalidPassword()
        {
            Assert.Throws<InvalidPasswordException>(() => 
                userService.Create("User", "user3", "abc", "abc", "abc", null));
            
            mockUserRepository.Verify(o => o.Get(It.IsAny<string>()), Times.Once);
            mockUserRepository.Verify(o => o.Add(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestCreateWithNotExistingRole()
        {
            Assert.Throws<RoleNotFoundException>(() => 
                userService.Create("Student", "user3", "abc", "abc", "Qwerty1234", null));
            
            mockUserRepository.Verify(o => o.Get(It.IsAny<string>()), Times.Once);
            mockUserRepository.Verify(o => o.Add(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestCreateWithCorrectData()
        {
            userService.Create("User", "user3", "Andrew", "Smith", "Qwerty1234", null);

            mockUserRepository.Verify(o => o.Get(It.IsAny<string>()), Times.Once);
            mockUserRepository.Verify(o => o.Add(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("Registrator")]
        [InlineData("Doctor")]
        [InlineData("LabTechnician")]
        [InlineData("LabManager")]
        public void TestCreateWithDifferentRoles(string role)
        {
            userService.Create(role, "user3", "Andrew", "Smith", "Qwerty1234", role == "Doctor" ? "1234" : null);
            
            mockUserRepository.Verify(o => o.Get(It.IsAny<string>()), Times.Exactly(2));
            mockUserRepository.Verify(o => o.Add(It.IsAny<User>()), Times.Once);

            switch (role)
            {
                case "Admin":
                    mockAdminRepository.Verify(o => o.Add(It.IsAny<Admin>()), Times.Once);
                    break;
                case "Registrator":
                    mockRegistratorRepository.Verify(o => o.Add(It.IsAny<Registrator>()), Times.Once);
                    break;
                case "Doctor":
                    mockDoctorRepository.Verify(o => o.Add(It.IsAny<Doctor>()), Times.Once);
                    break;
                case "LabTechnician":
                    mockLabTechnicianRepository.Verify(o => o.Add(It.IsAny<LabTechnician>()), Times.Once);
                    break;
                case "LabManager":
                    mockLabManagerRepository.Verify(o => o.Add(It.IsAny<LabManager>()), Times.Once);
                    break;
            }
        }
    }
}
