// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using API.Entities;
using API.Repositories;

namespace API.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Create(string login, string firstName, string lastName, string password, string? permitNumber)
        {
            throw new System.NotImplementedException();
        }

        public void Authenticate(string login, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
