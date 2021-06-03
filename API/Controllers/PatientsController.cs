using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService patientService;

        public PatientsController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        [HttpGet]
        [Authorize(Roles = "Registrar")]
        [ProducesResponseType(typeof(IEnumerable<Patient>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page, [FromQuery] int perPage)
        {
            return await patientService.GetAllAsync(page, perPage);
        }

        [HttpPost]
        [Authorize(Roles = "Registrar")]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        // below only when credentials are invalid
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RegisterPatientAsync([FromBody] PatientDto patientDto)
        {
            return await patientService.RegisterAsync(patientDto);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Registrar,Admin")]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdatePatientAsync(int id, [FromBody] PatientDto patientDto)
        {
            return await patientService.UpdateAsync(id, patientDto);
        }

        [HttpGet("suggestions")]
        [Authorize]
        [ProducesResponseType(typeof( SuggestionsDto), StatusCodes.Status200OK)]
        // below only when wrong role
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetSuggestionsAsync()
        {
            return await patientService.GetSuggestionsAsync();
        }
    }
}
