﻿using System;
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
        private readonly IPatientRepository patientRepository;
        private readonly IDoctorRepository doctorRepository;
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
        
        public IActionResult CreateAppointment(AppointmentDTO appointmentDto, HttpRequest request)
        {
            var doctor = doctorRepository.GetByPermitNumber(appointmentDto.PermitNumber);
            if (doctor == null)
            {
                return new JsonResult(new {message = "No doctor with given permit number was found"}){StatusCode = 422};
            }

            var patient = patientRepository.Get(appointmentDto.PeselNumber);
            if (patient == null)
            {
                return new JsonResult(new {message = "No patient with given identity number was found"}){StatusCode = 422};
            }

            if (string.IsNullOrEmpty(appointmentDto.Description))
            {
                return new JsonResult(new {message = "No appointment description was given"}){StatusCode = 422};
            }

            var timeNow = DateTime.Now;
            
            if (appointmentDto.ScheduledDate < timeNow)
            {
                return new JsonResult(new {message = "Given registration date is invalid"}){StatusCode = 422};
            }

            var registrator = userService.GetCurrentUser(request) as Registrator;

            if (registrator == null)
            {
                return new JsonResult(new {message = "Could not load registrator"}){StatusCode = 422};
            }
            
            var appointment = new Appointment()
            {
                RegistrationDate = timeNow,
                Description = appointmentDto.Description,
                Patient = patient,
                Doctor = doctor,
                Status = new ExaminationStatus() {Name = "SCHEDULED"},
                ScheduledDate = appointmentDto.ScheduledDate,
                Registrator = registrator,
            };
            
            appointmentRepository.Add(appointment);
            return new OkResult();
        }
    }
}