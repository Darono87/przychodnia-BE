using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<Appointment> GetAsync(int id);
    }
}
