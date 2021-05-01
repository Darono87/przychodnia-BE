using API.Entities;

namespace API.Repositories
{
    public interface IAppointmentRepository
    {
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        Appointment Get(int id);
    }
}
