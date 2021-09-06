using System.Threading.Tasks;
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

        public async Task<IActionResult> ConfirmLabExaminationAsync(ConfirmLabExaminationDto labExaminationDto)
        {
            var labExamination = await labExaminationRepository.GetAsync(labExaminationDto.Id);

            labExamination.Status = ExaminationStatus.Accepted;
            labExamination.ConfirmationDate = System.DateTime.Now;
            labExamination.ManagerRemarks = labExaminationDto.ManagerRemarks;

            await labExaminationRepository.UpdateAsync(labExamination);
            
            return new JsonResult(labExamination) {StatusCode = 200};
        }
        public async Task<IActionResult> ResultLabExaminationAsync(ResultLabExaminationDto labExaminationDto)
        {
            var labExamination = await labExaminationRepository.GetAsync(labExaminationDto.Id);

            labExamination.Status = ExaminationStatus.Finished;
            labExamination.FinishDate = System.DateTime.Now;
            labExamination.Result = labExaminationDto.Result;

            await labExaminationRepository.UpdateAsync(labExamination);
            
            return new JsonResult(labExamination) {StatusCode = 200};
        }

        public async Task<IActionResult> GetAllAsync(int[] appointments, ExaminationStatus[] statuses, int page, int perPage, bool isAscending, string sortKey)
        {

            return new JsonResult(await labExaminationRepository.GetAllAsync(appointments, statuses, page, perPage, isAscending, sortKey)) {StatusCode = 200};
        }
    }
}
