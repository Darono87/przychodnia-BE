using System.Threading.Tasks;
using API.DTO;
using API.Services;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExaminationCodesController : ControllerBase
    {
        private readonly IExaminationCodeService examinationCodeService;

        public ExaminationCodesController(IExaminationCodeService examinationCodeService)
        {
            this.examinationCodeService = examinationCodeService;
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,LabTechnician,LabManager,Admin")]
        [ProducesResponseType(typeof(ExaminationCode), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> AddExaminationCode([FromBody] ExaminationCodeDto examinationCodeDto )
        {
            return examinationCodeService.CreateExaminationCode(examinationCodeDto);
        }

        [HttpGet("suggestions")]
        [Authorize]
        [ProducesResponseType(typeof( SuggestionsDto), StatusCodes.Status200OK)]
        // below only when wrong role
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetSuggestionsAsync([FromQuery] ExaminationType examinationType)
        {
            return await examinationCodeService.GetSuggestionsAsync(examinationType);
        }
    }
}
