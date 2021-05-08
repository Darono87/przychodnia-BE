// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using API.DTO;
using API.Entities;
using API.Exceptions;
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

            mockUserRepository.Verify(o => o.GetAsync(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(o => o.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestCreateWithTakenLogin()
        {
            Assert.Throws<LoginTakenException>(() =>
                userService.Create("User", "user1", "abc", "abc", "abc", null));

            mockUserRepository.Verify(o => o.GetAsync(It.IsAny<string>()), Times.Once);
            mockUserRepository.Verify(o => o.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestCreateWithInvalidPassword()
        {
            Assert.Throws<InvalidPasswordException>(() =>
                userService.Create("User", "user3", "abc", "abc", "abc", null));

            mockUserRepository.Verify(o => o.GetAsync(It.IsAny<string>()), Times.Once);
            mockUserRepository.Verify(o => o.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestCreateWithNotExistingRole()
        {
            Assert.Throws<RoleNotFoundException>(() =>
                userService.Create("Student", "user3", "abc", "abc", "Qwerty1234", null));

            mockUserRepository.Verify(o => o.GetAsync(It.IsAny<string>()), Times.Once);
            mockUserRepository.Verify(o => o.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestCreateWithCorrectData()
        {
            userService.Create("User", "user3", "Andrew", "Smith", "Qwerty1234", null);

            mockUserRepository.Verify(o => o.GetAsync(It.IsAny<string>()), Times.Exactly(2));
            mockUserRepository.Verify(o => o.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("Registrar")]
        [InlineData("Doctor")]
        [InlineData("LabTechnician")]
        [InlineData("LabManager")]
        public void TestCreateWithDifferentRoles(string role)
        {
            userService.Create(role, "user3", "Andrew", "Smith", "Qwerty1234", role == "Doctor" ? "1234" : null);

            mockUserRepository.Verify(o => o.GetAsync(It.IsAny<string>()), Times.Exactly(2));
            mockUserRepository.Verify(o => o.AddAsync(It.IsAny<User>()), Times.Once);

            switch (role)
            {
                case "Admin":
                    mockAdminRepository.Verify(o => o.AddAsync(It.IsAny<Admin>()), Times.Once);
                    break;
                case "Registrar":
                    mockRegistrarRepository.Verify(o => o.AddAsync(It.IsAny<Registrar>()), Times.Once);
                    break;
                case "Doctor":
                    mockDoctorRepository.Verify(o => o.AddAsync(It.IsAny<Doctor>()), Times.Once);
                    break;
                case "LabTechnician":
                    mockLabTechnicianRepository.Verify(o => o.AddAsync(It.IsAny<LabTechnician>()), Times.Once);
                    break;
                case "LabManager":
                    mockLabManagerRepository.Verify(o => o.AddAsync(It.IsAny<LabManager>()), Times.Once);
                    break;
            }
        }

        [Fact]
        public void TestAuthenticateWithValidCredentials()
        {
            var expectedResult = new AuthenticationDTO
            {
                AccessToken = "access_token1", RefreshToken = "refresh_token1", Role = "User"
            };

            Assert.Equal(expectedResult, userService.Authenticate("user1", "Qwerty1234"));
            mockJwtManager.Verify(o => o.GenerateTokens(It.IsIn("user1"), It.IsAny<string>(), It.IsAny<DateTime>()),
                Times.Once);
        }

        [Theory]
        [InlineData("user1234", "Qwerty1234")]
        [InlineData("", "")]
        public void TestAuthenticateWithInvalidCredentials(string username, string password)
        {
            Assert.Throws<ArgumentException>(() => userService.Authenticate(username, password));
        }

        [Fact]
        public void TestGetRoleWithCorrectLogin()
        {
            Assert.Contains("AdminRegistrarDoctorLabTechnicianLabManager", userService.GetRole("user1"));
            mockUserRepository.Verify(o => o.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void TestGetRoleWithIncorrectLogin()
        {
            Assert.Throws<ArgumentException>(() => userService.GetRole(""));
        }

        [Fact]
        public void TestRefreshWithValidCredentials()
        {
            var expectedResult = new AuthenticationDTO
            {
                AccessToken = "access_token1", RefreshToken = "refresh_token1", Role = "User"
            };

            Assert.Equal(expectedResult, userService.Refresh("user1", "refresh_token1"));
            mockJwtManager.Verify(o => o.GenerateTokens(It.IsIn("user1"), It.IsAny<string>(), It.IsAny<DateTime>()),
                Times.Once);
        }

        [Theory]
        [InlineData("user1234", "Qwerty1234")]
        [InlineData("", "")]
        public void TestRefreshWithInvalidCredentials(string login, string refreshToken)
        {
            Assert.Throws<ArgumentException>(() => userService.Refresh(login, refreshToken));
        }
    }
}
