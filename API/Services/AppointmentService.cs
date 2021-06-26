using System;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly IPatientRepository patientRepository;
        private readonly IUserService userService;

        public AppointmentService(IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IUserService userService)
        {
            this.appointmentRepository = appointmentRepository;
            this.patientRepository = patientRepository;
            this.doctorRepository = doctorRepository;
            this.userService = userService;
        }

        public async Task<IActionResult> GetAllAppointmentsAsync(int page, int perPage, string peselNumber,
            string permitNumber)
        {
            if (!string.IsNullOrEmpty(peselNumber) || !string.IsNullOrEmpty(permitNumber))
            {
                return new JsonResult(
                    await appointmentRepository.GetAllFilteredAsync(page, perPage, peselNumber, permitNumber));
            }

            return new JsonResult(await appointmentRepository.GetAllAsync(page, perPage));
        }

        public async Task<IActionResult> CreateAppointmentAsync(AppointmentDto appointmentDto, HttpRequest request)
        {
            var doctor = await doctorRepository.GetAsync(appointmentDto.DoctorId);

            if (doctor == null)
            {
                return new JsonResult(new ExceptionDto {Message = "No doctor with given permit number was found"})
                {
                    StatusCode = 422
                };
            }

            var patient = await patientRepository.GetAsync(appointmentDto.PatientId);

            if (patient == null)
            {
                return new JsonResult(new ExceptionDto {Message = "No patient with given identity number was found"})
                {
                    StatusCode = 422
                };
            }

            var timeNow = DateTime.Now;

            if (appointmentDto.ScheduledDate < timeNow)
            {
                return new JsonResult(new ExceptionDto {Message = "Given registration date is invalid"})
                {
                    StatusCode = 422
                };
            }

            if (await userService.GetCurrentUserAsync(request) is not Registrar registrar)
            {
                return new JsonResult(new ExceptionDto {Message = "Could not find registrar"}) {StatusCode = 422};
            }

            var appointment = new Appointment
            {
                RegistrationDate = timeNow,
                Description = appointmentDto.Description,
                Patient = patient,
                Doctor = doctor,
                Status = AppointmentStatus.Scheduled,
                ScheduledDate = appointmentDto.ScheduledDate,
                Registrar = registrar
            };

            return new JsonResult(await appointmentRepository.AddAsync(appointment)) {StatusCode = 201};
        }

        public async Task<IActionResult> CancelAppointmentAsync(AppointmentCancellationDto appointmentCancellationDto)
        {
            var appointment = await appointmentRepository.GetAsync(appointmentCancellationDto.Id);

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.FinishDate = DateTime.Now;

            if (!string.IsNullOrEmpty(appointmentCancellationDto.Description))
            {
                appointment.Description = appointmentCancellationDto.Description;
            }

            return new JsonResult(await appointmentRepository.UpdateAsync(appointment)) {StatusCode = 200};
        }
    }
}
