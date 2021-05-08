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
    public class RegistrarRepository : IGenericUserRepository<Registrar>
    {
        private readonly DataContext context;

        public RegistrarRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Registrar> AddAsync(Registrar obj)
        {
            var result = context.Registrars.Add(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Registrar> UpdateAsync(Registrar obj)
        {
            var result = context.Registrars.Update(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Registrar> GetAsync(int id)
        {
            return await context.Registrars.FindAsync(id);
        }

        public async Task<Registrar> GetAsync(string login)
        {
            return await context.Registrars
                .FirstOrDefaultAsync(a =>
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
