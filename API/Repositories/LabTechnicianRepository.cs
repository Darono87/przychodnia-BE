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
    public class LabTechnicianRepository : IGenericUserRepository<LabTechnician>
    {
        private readonly DataContext context;

        public LabTechnicianRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<LabTechnician> AddAsync(LabTechnician obj)
        {
            var result = context.LabTechnicians.Add(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<LabTechnician> UpdateAsync(LabTechnician obj)
        {
            var result = context.LabTechnicians.Update(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<LabTechnician> GetAsync(int id)
        {
            return await context.LabTechnicians.FindAsync(id);
        }

        public async Task<LabTechnician> GetAsync(string login)
        {
            return await context.LabTechnicians
                .FirstOrDefaultAsync(a =>
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
