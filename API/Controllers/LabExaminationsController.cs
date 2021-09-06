using System.Collections.Generic;
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
    public class LabExaminationsController : ControllerBase
    {
        private readonly ILabExaminationService labExaminationService;

        public LabExaminationsController(ILabExaminationService labExaminationService)
        {
            this.labExaminationService = labExaminationService;
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(typeof(LabExamination), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> AddPhysicalExamination([FromBody] CreateLabExaminationDto labExaminationDto)
        {
            return labExaminationService.CreateLabExaminationAsync(labExaminationDto);
        }

        [HttpPut("finalize")]
        [Authorize(Roles = "LabManager")]
        [ProducesResponseType(typeof(LabExamination), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> ConfirmLabExamination([FromBody] ConfirmLabExaminationDto labExaminationDto)
        {
            return labExaminationService.ConfirmLabExaminationAsync(labExaminationDto);
        }
        
        [HttpPut("confirm")]
        [Authorize(Roles = "LabManager,LabTechnician")]
        [ProducesResponseType(typeof(LabExamination), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> ConfirmLabExamination([FromBody] ResultLabExaminationDto labExaminationDto)
        {
            return labExaminationService.ResultLabExaminationAsync(labExaminationDto);
        }
        
        [HttpGet]
        [Authorize(Roles = "Doctor,LabTechnician,LabManager")]
        [ProducesResponseType(typeof(IEnumerable<PaginationDTO<LabExamination>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int perPage, 
        [FromQuery] bool isAscending, [FromQuery] string sortKey, [FromQuery(Name = "appointments[]")] int[] appointments, [FromQuery] ExaminationStatus[] statuses)
        {
            return labExaminationService.GetAllAsync(appointments, statuses, page, perPage, isAscending, sortKey);
        }
    }
}
