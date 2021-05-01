// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using API.DTO;
using API.Entities;
using API.Exceptions;
using API.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("test")]
        public IActionResult GetInfo()
        {
            var res = userService.GetCurrentUser(Request);
            return new JsonResult(new {message = res.GetType() });
        }
        
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] UserCreationDTO userDto)
        {
            try
            {
                userService.Create(userDto.Role, userDto.Login, userDto.FirstName, userDto.LastName, userDto.Password,
                    userDto.PermitNumber);
            }
            catch (Exception e)
            {
                return new JsonResult(new {message = e.Message}) {StatusCode = 422};
            }

            return new OkResult();
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] LoginDTO loginDto)
        {
            try
            {
                var response = userService.Authenticate(loginDto.Login, loginDto.Password);
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                return new JsonResult(new {message = e.Message}) {StatusCode = 422};
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult Refresh([FromBody] RefreshDTO refreshDto)
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

