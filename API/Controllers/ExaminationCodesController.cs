using System.Threading.Tasks;
using API.DTO;
using API.Services;
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

        [HttpGet("suggestions")]
        [Authorize]
        [ProducesResponseType(typeof( SuggestionsDto), StatusCodes.Status200OK)]
        // below only when wrong role
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetSuggestionsAsync()
        {
            return await examinationCodeService.GetSuggestionsAsync();
        }
    }
}
