// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] UserCreationDto userDto)
        {
            return new JsonResult(await userService.Create(userDto.Role, userDto.Login, userDto.FirstName,
                userDto.LastName,
                userDto.Password,
                userDto.PermitNumber));
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            return new JsonResult(await userService.Authenticate(loginDto.Login, loginDto.Password));
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult Refresh([FromBody] RefreshDto refreshDto)
        {
            try
            {
                var response = userService.Refresh(refreshDto.AccessToken, refreshDto.RefreshToken);
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                return new JsonResult(new {message = e.Message}) {StatusCode = 422};
            }
        }
    }
}
