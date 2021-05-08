using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DataContext context;

        public AppointmentRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add(Appointment appointment)
        {
            context.Appointments.Add(appointment);
            context.SaveChanges();
        }

        public void Update(Appointment appointment)
        {
            context.Appointments.Update(appointment);
            context.SaveChanges();
        }

        public Appointment Get(int id)
        {
            return context.Appointments.Find(id);
        }
    }
}
