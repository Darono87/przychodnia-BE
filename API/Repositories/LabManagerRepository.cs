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
    public class LabManagerRepository : IGenericUserRepository<LabManager>
    {
        private readonly DataContext context;

        public LabManagerRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<LabManager> AddAsync(LabManager obj)
        {
            var result = context.LabManagers.Add(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<LabManager> UpdateAsync(LabManager obj)
        {
            var result = context.LabManagers.Update(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<LabManager> GetAsync(int id)
        {
            return await context.LabManagers.FindAsync(id);
        }

        public async Task<LabManager> GetAsync(string login)
        {
            return await context.LabManagers
                .FirstOrDefaultAsync(a =>
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
