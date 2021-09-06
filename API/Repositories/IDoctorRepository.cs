using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface IDoctorRepository
    {

        public Task<Doctor> GetAsync(int id);

        public Task<Doctor> GetByPermitNumberAsync(string permitNumber);
    }
}
