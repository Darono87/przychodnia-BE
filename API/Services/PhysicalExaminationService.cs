using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class PhysicalExaminationService : IPhysicalExaminationService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IExaminationCodeRepository examinationCodeRepository;
        private readonly IPhysicalExaminationRepository physicalExaminationRepository;

        public PhysicalExaminationService(IAppointmentRepository appointmentRepository,
            IPhysicalExaminationRepository physicalExaminationRepository,
            IExaminationCodeRepository examinationCodeRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.physicalExaminationRepository = physicalExaminationRepository;
            this.examinationCodeRepository = examinationCodeRepository;
        }


        public async Task<IActionResult> CreatePhysicalExaminationAsync(PhysicalExaminationDTO physicalExaminationDto,
            HttpRequest request)
        {
            if (string.IsNullOrEmpty(physicalExaminationDto.Result))
            {
                return new JsonResult(new ExceptionDto {Message = "No examination result was passed"})
                {
                    StatusCode = 422
                };
            }

            var examinationCode =
                await examinationCodeRepository.GetAsync(physicalExaminationDto.ExaminationCodeId);

            if (examinationCode == null)
            {
                return new JsonResult(new ExceptionDto {Message = "No examination with given code was found"})
                {
                    StatusCode = 422
                };
            }

            var appointment = await appointmentRepository.GetAsync(physicalExaminationDto.AppointmentId);

            if (appointment == null)
            {
                return new JsonResult(new ExceptionDto {Message = "Could not find the appointment"}) {StatusCode = 422};
            }

            var physicalExamination = new PhysicalExamination
            {
                Result = physicalExaminationDto.Result, Appointment = appointment, ExaminationCode = examinationCode
            };

            await physicalExaminationRepository.AddAsync(physicalExamination);

            appointment.PhysicalExaminations.Add(physicalExamination);
            await appointmentRepository.UpdateAsync(appointment);

            return new JsonResult(physicalExamination) {StatusCode = 200};
        }

        public async Task<IActionResult> GetAllAsync(int[] appointments, int page, int perPage, bool isAscending, string sortKey)
        {
            return new JsonResult(await physicalExaminationRepository.GetAllAsync(appointments, page, perPage, isAscending, sortKey)) {StatusCode = 200};
        }
    }
}
