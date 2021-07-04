using System.Collections.Generic;
using System.Net.Http.Headers;
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
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Registrar,Doctor")]
        [ProducesResponseType(typeof(IEnumerable<Appointment>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAppointmentsAsync([FromQuery] int page, [FromQuery] int perPage,
            [FromQuery] string peselNumber, [FromQuery] string permitNumber)
        {
            return await appointmentService.GetAllAppointmentsAsync(page, perPage, peselNumber, permitNumber);
        }

        [HttpPost]
        [Authorize(Roles = "Registrar")]
        [ProducesResponseType(typeof(Appointment), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        // below only when credentials are invalid
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddAppointmentAsync([FromBody] AppointmentDto appointmentDto)
        {
            return await appointmentService.CreateAppointmentAsync(appointmentDto, Request);
        }

        [HttpPatch("cancel")]
        [Authorize(Roles = "Registrar,Doctor")]
        [ProducesResponseType(typeof(Appointment), StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelAppointmentAsync(
            [FromBody] AppointmentCancellationDto appointmentCancellationDto)
        {
            return await appointmentService.CancelAppointmentAsync(appointmentCancellationDto);
        }

        [HttpGet("suggestions")]
        [Authorize]
        [ProducesResponseType(typeof( SuggestionsDto), StatusCodes.Status200OK)]
        // below only when wrong role
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetSuggestionsAsync()
        {
            return await appointmentService.GetSuggestionsAsync(Request);
        }
    }
}
