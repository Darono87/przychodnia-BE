// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using API.Entities;
using API.Repositories;
using API.Services;
using Moq;
using Xunit;

namespace API.Tests.Services
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> mockUserRepository;
        private IUserRepository userRepository;
        private IUserService userService;

        private void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();
            
            mockUserRepository.Setup(o => o.Get(It.IsIn(1)))
                .Returns(new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe"});
            mockUserRepository.Setup(o => o.Get(It.IsIn(2)))
                .Returns(new User() {Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith"});
            mockUserRepository.Setup(o => o.Get(It.IsIn("user1")))
                .Returns(new User() {Id = 1, Login = "user1", FirstName = "John", LastName = "Doe"});
            mockUserRepository.Setup(o => o.Get(It.IsIn("user2")))
                .Returns(new User() {Id = 2, Login = "user2", FirstName = "Jane", LastName = "Smith"});

            mockUserRepository.Setup(o => o.Add(It.IsIn<User>(null)))
                .Throws(new ArgumentException());
            mockUserRepository.Setup(o => o.Add(It.IsNotNull<User>()));

            mockUserRepository.Setup(o => o.Update(It.IsAny<User>()));

            userRepository = mockUserRepository.Object;
        }

        public UserServiceTests()
        {
            Setup();
            userService = new UserService(userRepository);
        }

        [Fact]
        public void TestCreateWithNulls()
        {
            Assert.Throws<ArgumentException>(() => userService.Create("", "", "", "", null));
            
            mockUserRepository.Verify(o => o.Get(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(o => o.Add(It.IsAny<User>()), Times.Never);
        }
    }
}
