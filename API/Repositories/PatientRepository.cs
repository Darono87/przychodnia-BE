using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
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

        public async Task<PaginationDTO<Patient>> GetAllAsync(int page, int perPage, bool isAscending, string sortKey)
        {

            var count = await context.Patients.CountAsync();
            var currentPage = page > 0 ? page : 1;
            var itemCount = perPage > 0 ? perPage : 20;

            var patientContext = context.Patients.Include(patient => patient.Address);

            var sortedPatients = sortKey switch{
                "peselNumber" =>isAscending ? patientContext.OrderBy(p=>p.PeselNumber) : patientContext.OrderByDescending(p=>p.PeselNumber),
                "country" => isAscending ? patientContext.OrderBy(p=>p.Address.Country) : patientContext.OrderByDescending(p=>p.Address.Country),
                "address" => isAscending ? patientContext.OrderBy(p=>p.Address.City).ThenBy(p=>p.Address.Street).ThenBy(p=>p.Address.BuildingNumber) : patientContext.OrderByDescending(p=>p.Address.City).ThenByDescending(p=>p.Address.Street).ThenByDescending(p=>p.Address.BuildingNumber),
                _ => isAscending ? patientContext.OrderBy(p=>p.FirstName).ThenBy(p=>p.LastName) : patientContext.OrderByDescending(p=>p.FirstName).ThenByDescending(p=>p.LastName)
            };

            var patients = await Task.FromResult(sortedPatients.Skip(itemCount * (currentPage - 1)).Take(itemCount).ToArray());

            return new PaginationDTO<Patient>(){
                count = count,
                items = patients
            };
        }

        public async Task<Patient[]> GetAllAsync()
        {

            var patients = await context.Patients.ToArrayAsync();
            return patients;
        }
    }
}
