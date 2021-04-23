// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext context;

        public UserRepository(DataContext context)
        {
            this.context = context;
        }
        
        public void Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Update(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }

        public User Get(int id)
        {
            return context.Users.Find(id);
        }

        public User Get(string login)
        {
            return context.Users.FirstOrDefault(user => user.Login == login);
        }
    }
}
