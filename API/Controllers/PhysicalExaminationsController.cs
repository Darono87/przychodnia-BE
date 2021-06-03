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
    public class PhysicalExaminationsController : ControllerBase
    {
        private readonly IPhysicalExaminationService physicalExaminationService;

        public PhysicalExaminationsController(IPhysicalExaminationService physicalExaminationService)
        {
            this.physicalExaminationService = physicalExaminationService;
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(typeof(PhysicalExamination), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> AddPhysicalExamination([FromBody] PhysicalExaminationDTO physicalExaminationDto)
        {
            return physicalExaminationService.CreatePhysicalExaminationAsync(physicalExaminationDto, Request);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(typeof(IEnumerable<PhysicalExamination>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public Task<IActionResult> GetAllForAppointment(int id)
        {
            return physicalExaminationService.GetAllAsync(id);
        }
    }
}
