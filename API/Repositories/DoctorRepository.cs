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
    public class DoctorRepository : IGenericUserRepository<Doctor>, IDoctorRepository
    {
        private readonly DataContext context;

        public DoctorRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Doctor> GetByPermitNumberAsync(string permitNumber)
        {
            return await context.Doctors
                .FirstOrDefaultAsync(a =>
                    a.PermitNumber == permitNumber);
        }

        public async Task<Doctor> AddAsync(Doctor obj)
        {
            var result = context.Doctors.Add(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Doctor> UpdateAsync(Doctor obj)
        {
            var result = context.Doctors.Update(obj);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Doctor> GetAsync(int id)
        {
            return await context.Doctors.FindAsync(id);
        }

        public async Task<Doctor[]> GetAllAsync()
        {
            return await context.Doctors.Include(d=>d.User).ToArrayAsync();
        }

        public async Task<Doctor> GetAsync(string login)
        {
            return await context.Doctors
                .FirstOrDefaultAsync(a =>
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
