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

        [HttpPost]
        [Authorize(Roles = "Registrar")]
        [ProducesResponseType(typeof(Appointment), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        // below only when credentials are invalid
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddAppointmentAsync([FromBody] AppointmentDto appointmentDto)
        {
            return await appointmentService.CreateAppointmentAsync(appointmentDto, Request);
        }
    }
}
