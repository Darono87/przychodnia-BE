using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Appointment>> GetAllAsync(int page, int perPage)
        {
            var currentPage = page > 0 ? page : 1;
            var itemCount = perPage > 0 ? perPage : 20;

            return await Task.FromResult(context.Appointments.Skip(itemCount * (currentPage - 1)).Take(itemCount)
                .AsEnumerable());
        }

        public async Task<IEnumerable<Appointment>> GetAllFilteredAsync(int page, int perPage, string peselNumber,
            string permitNumber)
        {
            var currentPage = page > 0 ? page : 1;
            var itemCount = perPage > 0 ? perPage : 20;

            if (!string.IsNullOrEmpty(peselNumber) && !string.IsNullOrEmpty(permitNumber))
            {
                return await Task.FromResult(context.Appointments
                    .Where(appointment => appointment.Patient.PeselNumber.ToLower().Equals(peselNumber.ToLower()) &&
                                          appointment.Doctor.PermitNumber.ToLower().Equals(permitNumber.ToLower()))
                    .Skip(itemCount * (currentPage - 1)).Take(itemCount));
            }

            if (!string.IsNullOrEmpty(peselNumber) && string.IsNullOrEmpty(permitNumber))
            {
                return await Task.FromResult(context.Appointments
                    .Where(appointment => appointment.Patient.PeselNumber.ToLower().Equals(peselNumber.ToLower()))
                    .Skip(itemCount * (currentPage - 1)).Take(itemCount));
            }

            return await Task.FromResult(context.Appointments
                .Where(appointment => appointment.Doctor.PermitNumber.ToLower().Equals(permitNumber.ToLower()))
                .Skip(itemCount * (currentPage - 1)).Take(itemCount));
        }
    }
}
