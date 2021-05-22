using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<Appointment> GetAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAsync(int page, int perPage);

        Task<IEnumerable<Appointment>> GetAllFilteredAsync(int page, int perPage, string peselNumber,
            string permitNumber);
    }
}
