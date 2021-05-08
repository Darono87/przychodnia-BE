using System.Threading.Tasks;
using API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IAppointmentService
    {
        Task<IActionResult> CreateAppointment(AppointmentDto appointmentDto, HttpRequest request);
    }
}
