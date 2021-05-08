using API.DTO;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Registrar")]
        public IActionResult AddAppointment([FromBody] AppointmentDTO appointmentDto)
        {
            return appointmentService.CreateAppointment(appointmentDto, Request);
        }
    }
}
