// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using API.DTO;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailsController : ControllerBase
    {
        
        private readonly IMailService mailService;

        public MailsController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(MailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> SendMailAsync([FromBody] MailDto mailDto)
        {
            return await mailService.SendAsync(mailDto);
        }
    }
}
