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

        [HttpPut]
        [Authorize(Roles = "LabManager")]
        [ProducesResponseType(typeof(LabExamination), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> ConfirmLabExamination([FromBody] ConfirmLabExaminationDto labExaminationDto)
        {
            return labExaminationService.ConfirmLabExaminationAsync(labExaminationDto);
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(typeof(IEnumerable<PhysicalExamination>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> GetAllForAppointment(int id, [FromQuery] int page, [FromQuery] int perPage, 
        [FromQuery] bool isAscending, [FromQuery] string sortKey)
        {
            return labExaminationService.GetAllAsync(id, page, perPage, isAscending, sortKey);
        }
    }
}
