using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface IDoctorRepository
    {
        public Task<Doctor> GetByPermitNumberAsync(string permitNumber);
    }
}
