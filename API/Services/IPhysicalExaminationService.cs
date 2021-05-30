using System.Threading.Tasks;
using API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IPhysicalExaminationService
    {
        Task<IActionResult> CreatePhysicalExaminationAsync(PhysicalExaminationDTO physicalExaminationDto,
            HttpRequest request);

        Task<IActionResult> GetAllAsync(int appointmentId);
    }
}
