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

        public async Task<IEnumerable<Patient>> GetAllAsync(int page, int perPage)
        {
            var currentPage = page > 0 ? page : 1;
            var itemCount = perPage > 0 ? perPage : 20;

            return await Task.FromResult(context.Patients.Skip(itemCount * (currentPage - 1)).Take(itemCount)
                .Include(patient => patient.Address));
        }
    }
}
