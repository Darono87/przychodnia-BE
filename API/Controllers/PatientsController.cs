using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;

        public PatientsController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Registrar")]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        // below only when credentials are invalid
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult RegisterPatientAsync([FromBody] PatientDto patientDto)
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
