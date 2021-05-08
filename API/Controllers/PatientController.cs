using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin,Registrar")]
        public IActionResult RegisterPatient([FromBody] PatientDto patientDto)
        {
            if (patientRepository.Get(patientDto.PeselNumber) != null)
            {
                return new JsonResult(new {message = "Patient with given identity number already exists"})
                {
                    StatusCode = 422
                };  
            }

            var patient = new Patient
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                PeselNumber = patientDto.PeselNumber,
                Address = new Address
                {
                    Country = patientDto.Country,
                    City = patientDto.City,
                    BuildingNumber = patientDto.BuildingNumber,
                    PostalCode = patientDto.PostalCode,
                    Street = patientDto.Street
                }
            };

            patientRepository.Add(patient);
            return new OkResult();
        }
    }
}
