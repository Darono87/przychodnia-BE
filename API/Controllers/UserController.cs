// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using API.DTO;
using API.Exceptions;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        
        [HttpPost("create")]
        public IActionResult Create([FromBody] UserCreationDTO userDto)
        {
            try
            {
                userService.Create(userDto.Role, userDto.Login, userDto.FirstName, userDto.LastName, userDto.Password, userDto.PermitNumber);
            }
            catch (Exception ae)
            {
                return new JsonResult(new { message = ae.Message }){ StatusCode = 422 };
            }

            return new OkResult();
        }
    }
}
