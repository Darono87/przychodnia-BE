// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AdminRepository : IGenericUserRepository<Admin>
    {
        private readonly DataContext context;

        public AdminRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Admin> AddAsync(Admin obj)
        {
            var result = context.Admins.Add(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Admin> UpdateAsync(Admin obj)
        {
            var result = context.Admins.Update(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Admin> GetAsync(int id)
        {
            return await context.Admins.FindAsync(id);
        }

        public async Task<Admin> GetAsync(string login)
        {
            return await context.Admins
                .FirstOrDefaultAsync(a =>
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
