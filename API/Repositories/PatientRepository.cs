using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataContext context;

        public PatientRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            var result = context.Patients.Add(patient);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Patient> UpdateAsync(Patient patient)
        {
            var result = context.Patients.Update(patient);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Patient> GetAsync(int id)
        {
            return await context.Patients.Include(patient => patient.Address).FirstAsync(patient => patient.Id == id);
        }

        public async Task<Patient> GetAsync(string PeselNumber)
        {
            return await context.Patients
                .FirstOrDefaultAsync(a =>
                    a.PeselNumber == PeselNumber);
        }

        public async Task<Patient[]> GetAllAsync(int? page, int? perPage)
        {
            if(page is int pageInt && perPage is int perPageInt) {
                var currentPage = pageInt > 0 ? pageInt : 1;
                var itemCount = perPageInt > 0 ? perPageInt : 20;

                return await Task.FromResult(context.Patients.Skip(itemCount * (currentPage - 1)).Take(itemCount)
                    .Include(patient => patient.Address).ToArray());
            } else { 
                return await context.Patients.ToArrayAsync();
            }
        }
    }
}
