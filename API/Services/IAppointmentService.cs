using System.Threading.Tasks;
using API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IAppointmentService
    {
        Task<IActionResult> GetAllAppointmentsAsync(int page, int perPage, string peselNumber, string permitNumber, HttpRequest request, bool isAscending, string sortKey);
        Task<IActionResult> CreateAppointmentAsync(AppointmentDto appointmentDto, HttpRequest request);
        Task<IActionResult> CancelAppointmentAsync(AppointmentCancellationDto appointmentCancellationDto);

        Task<IActionResult> GetSuggestionsAsync(HttpRequest request);
    }
}
