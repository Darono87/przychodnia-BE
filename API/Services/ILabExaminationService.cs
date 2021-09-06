using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface ILabExaminationService
    {
        Task<IActionResult> CreateLabExaminationAsync(CreateLabExaminationDto labExaminationDto);

        Task<IActionResult> ConfirmLabExaminationAsync(ConfirmLabExaminationDto labExaminationDto);
        
        Task<IActionResult> ResultLabExaminationAsync(ResultLabExaminationDto labExaminationDto);

        Task<IActionResult> GetAllAsync(int[] appointments, ExaminationStatus[] statuses, int page, int perPage, bool isAscending, string sortKey);
    }
}
