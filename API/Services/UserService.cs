// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using System.Linq;
using API.Entities;
using API.Exceptions;
using API.Repositories;
using API.Utils;

namespace API.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private string[] roles = 
        {
            typeof(User).ToString(), typeof(Registrator).ToString(), typeof(Doctor).ToString(),
            typeof(LabTechnician).ToString(), typeof(LabManager).ToString()
        };

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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

            if (!roles.Any(r => r.Contains(role)))
            {
                throw new RoleNotFoundException();
            }

            if (!Helper.ValidatePassword(password))
            {
                throw new InvalidPasswordException();
            }

            if (role == "User")
            {
                userRepository.Add(new User
                {
                    FirstName = firstName,
                    LastName =  lastName,
                    Login = login,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
                });
            }
        }

        public void Authenticate(string login, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
