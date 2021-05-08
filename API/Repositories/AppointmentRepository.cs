using System.Threading.Tasks;
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

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            var result = context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            var result = context.Appointments.Update(appointment);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Appointment> GetAsync(int id)
        {
            return await context.Appointments.FindAsync(id);
        }
    }
}
