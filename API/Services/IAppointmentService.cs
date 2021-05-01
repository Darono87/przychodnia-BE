using API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IAppointmentService
    {
        IActionResult CreateAppointment(AppointmentDTO appointmentDto,HttpRequest request);
    }
}
