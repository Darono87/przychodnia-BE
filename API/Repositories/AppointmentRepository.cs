using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using static API.DTO.SuggestionsDto;

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
            var result = await context.Appointments.AddAsync(appointment);
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

        public async Task<PaginationDTO<Appointment>> GetAllAsync(int page, int perPage, int? doctorId,
            bool isAscending, string sortKey)
        {
            var currentPage = page > 0 ? page : 1;
            var itemCount = perPage > 0 ? perPage : 20;

            var appointmentsContext = context.Appointments
                .Where(appointment => doctorId == null || appointment.Doctor.Id == doctorId)
                .Include(appointment => appointment.Doctor.User)
                .Include(appointment => appointment.Patient);

            Func<Appointment, object> orderFun = sortKey switch
            {
                "status" => a => a.Status,
                "doctor" => a => a.Doctor.User.FirstName,
                "patient" => a => a.Patient.FirstName,
                "registrationDate" => a => a.RegistrationDate,
                "scheduledDate" => a => a.ScheduledDate,
                "finishDate" => a => a.FinishDate,
                "diagnosis" => a => a.Diagnosis,
                _ => a => a.Description
            };

            var appointmentsSorted = appointmentsContext.OrderBy(orderFun);
            if (!isAscending)
            {
                appointmentsSorted = appointmentsContext.OrderByDescending(orderFun);
                if (sortKey == "doctor")
                {
                    appointmentsSorted = appointmentsSorted.ThenByDescending(a => a.Doctor.User.LastName);
                }

                if (sortKey == "patient")
                {
                    appointmentsSorted = appointmentsSorted.ThenByDescending(a => a.Patient.LastName);
                }
            }
            else
            {
                if (sortKey == "doctor")
                {
                    appointmentsSorted = appointmentsSorted.ThenBy(a => a.Doctor.User.LastName);
                }

                if (sortKey == "patient")
                {
                    appointmentsSorted = appointmentsSorted.ThenBy(a => a.Patient.LastName);
                }
            }

            var appointments = await Task.FromResult(
                appointmentsSorted
                    .Skip(itemCount * (currentPage - 1))
                    .Take(itemCount)
                    .AsEnumerable());

            var count = await Task.FromResult(context.Appointments
                .Where(appointment => doctorId == null || appointment.Doctor.Id == doctorId)
                .Count());

            appointments = appointments.Select(appointment =>
            {
                appointment.Doctor.User.PasswordHash = null;

                var physicalExaminations =
                    context.PhysicalExaminations
                        .Where(pe => pe.Appointment.Id == appointment.Id)
                        .Include(pe => pe.Appointment);
                var allPhysicalFinished = physicalExaminations.All(pe => !string.IsNullOrEmpty(pe.Result));
                var labExaminations = context.LabExaminations
                    .Where(le => le.Appointment.Id == appointment.Id)
                    .Include(le => le.Appointment);
                var allLabFinished = labExaminations.All(le => le.Status == ExaminationStatus.Finished);

                appointment.IsFinishable = (physicalExaminations.Any() || labExaminations.Any()) &&
                                           (
                                               physicalExaminations.Any() && allPhysicalFinished ||
                                               labExaminations.Any() && allLabFinished
                                           );

                return appointment;
            });

            return new PaginationDTO<Appointment> {items = appointments, count = count};
        }

        public async Task<SuggestionsDto> GetSuggestionsAsync(int doctorId)
        {
            var suggestions = await context.Appointments
                .Where(a => a.Doctor.Id == doctorId)
                .Select(a => new Suggestion
                {
                    value = a.Id,
                    label = a.ScheduledDate.ToUniversalTime() + ", " + a.Patient.FirstName + " " +
                            a.Patient.LastName
                }).ToListAsync();
            return new SuggestionsDto {Suggestions = suggestions};
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
