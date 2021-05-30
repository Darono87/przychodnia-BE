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
        private readonly IPhysicalExaminationRepository physicalExaminationRepository;

        public PhysicalExaminationService(IAppointmentRepository appointmentRepository,
            IPhysicalExaminationRepository physicalExaminationRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.physicalExaminationRepository = physicalExaminationRepository;
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

            if (string.IsNullOrEmpty(physicalExaminationDto.ExaminationCodeAbbreviation))
            {
                return new JsonResult(new ExceptionDto {Message = "No examination code was passed"})
                {
                    StatusCode = 422
                };
            }

            var appointment = await appointmentRepository.GetAsync(physicalExaminationDto.AppointmentId);

            if (appointment == null)
            {
                return new JsonResult(new ExceptionDto {Message = "Could not find the appointment"})
                {
                    StatusCode = 422
                };
            }

            var physicalExamination = new PhysicalExamination() { };
        }

        public Task<IActionResult> GetAllAsync(Appointment appointment)
        {
            
        }
    }
}
