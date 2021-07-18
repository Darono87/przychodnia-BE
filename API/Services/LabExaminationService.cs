﻿using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class LabExaminationService : ILabExaminationService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IExaminationCodeRepository examinationCodeRepository;
        private readonly ILabExaminationRepository labExaminationRepository;

        public LabExaminationService(IAppointmentRepository appointmentRepository,
            ILabExaminationRepository labExaminationRepository,
            IExaminationCodeRepository examinationCodeRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.labExaminationRepository = labExaminationRepository;
            this.examinationCodeRepository = examinationCodeRepository;
        }


        public async Task<IActionResult> CreateLabExaminationAsync(CreateLabExaminationDto labExaminationDto)
        {

            var examinationCode =
                await examinationCodeRepository.GetAsync(labExaminationDto.ExaminationCodeId);

            if (examinationCode == null)
            {
                return new JsonResult(new ExceptionDto {Message = "No examination with given code was found"})
                {
                    StatusCode = 422
                };
            }

            var appointment = await appointmentRepository.GetAsync(labExaminationDto.AppointmentId);

            if (appointment == null)
            {
                return new JsonResult(new ExceptionDto {Message = "Could not find the appointment"}) {StatusCode = 422};
            }

            var labExamination = new LabExamination
            {
                Appointment = appointment, ExaminationCode = examinationCode, Status=ExaminationStatus.Scheduled, IssueDate= System.DateTime.Now
            };

            await labExaminationRepository.AddAsync(labExamination);
            appointment.LabExaminations.Add(labExamination);
            await appointmentRepository.UpdateAsync(appointment);

            return new JsonResult(labExamination) {StatusCode = 200};
        }

        public async Task<IActionResult> GetAllAsync(int appointmentId, int page, int perPage, bool isAscending, string sortKey)
        {
            var appointment = await appointmentRepository.GetAsync(appointmentId);
            if (appointment == null)
            {
                return new JsonResult(new ExceptionDto {Message = "Could not find the appointment"}) {StatusCode = 422};
            }

            return new JsonResult(await labExaminationRepository.GetAllAsync(appointment, page, perPage, isAscending, sortKey)) {StatusCode = 200};
        }
    }
}
