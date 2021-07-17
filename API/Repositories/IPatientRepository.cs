using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.DTO;

namespace API.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);
        Task<Patient> UpdateAsync(Patient patient);
        Task<Patient> GetAsync(int id);
        Task<Patient> GetAsync(string PeselNumber);
        Task<PaginationDTO<Patient>> GetAllAsync(int page, int perPage, bool isAscending, string sortKey);
        Task<Patient[]> GetAllAsync();
    }
}
