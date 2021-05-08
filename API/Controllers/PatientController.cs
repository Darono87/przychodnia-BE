using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController
    {
        private readonly IPatientRepository patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin,Registrar")]
        public IActionResult RegisterPatient([FromBody] PatientDTO patientDTO)
        {
            if (patientRepository.Get(patientDTO.PeselNumber) != null)
            {
                return new JsonResult(new {message = "Patient with given identity number already exists"})
                {
                    StatusCode = 422
                };
            }

            var patient = new Patient
            {
                FirstName = patientDTO.FirstName,
                LastName = patientDTO.LastName,
                PeselNumber = patientDTO.PeselNumber,
                Address = new Address
                {
                    Country = patientDTO.Country,
                    City = patientDTO.City,
                    BuildingNumber = patientDTO.BuildingNumber,
                    PostalCode = patientDTO.PostalCode,
                    Street = patientDTO.Street
                }
            };

            patientRepository.Add(patient);
            return new OkResult();
        }
    }
}
