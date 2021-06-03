// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : IGenericUserRepository<User>
    {
        private readonly DataContext context;

        public UserRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            var result = context.Users.Add(user);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<User[]> GetAllAsync()
        {
            return await context.Users.ToArrayAsync();
        }

        public async Task<User> UpdateAsync(User user)
        {
            var result = context.Users.Update(user);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<User> GetAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> GetAsync(string login)
        {
            return await context.Users.FirstOrDefaultAsync(user => user.Login == login);
        }
    }
}
