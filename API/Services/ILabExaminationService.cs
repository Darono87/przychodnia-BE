using System.Threading.Tasks;
using API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface ILabExaminationService
    {
        Task<IActionResult> CreateLabExaminationAsync(CreateLabExaminationDto labExaminationDto);

        Task<IActionResult> ConfirmLabExaminationAsync(ConfirmLabExaminationDto labExaminationDto);

        Task<IActionResult> GetAllAsync(int appointmentId, int page, int perPage, bool isAscending, string sortKey);
    }
}
