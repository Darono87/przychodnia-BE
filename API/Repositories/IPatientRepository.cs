using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);
        Task<Patient> UpdateAsync(Patient patient);
        Task<Patient> GetAsync(int id);
        Task<Patient> GetAsync(string PeselNumber);
        Task<Patient[]> GetAllAsync(int? page = null, int? perPage = null);
    }
}
