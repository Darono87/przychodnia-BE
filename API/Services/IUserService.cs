// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IUserService
    {
        Task<IActionResult> CreateAsync(string role, string login, string firstName, string lastName, string password,
            string? permitNumber);

        Task<IActionResult> AuthenticateAsync(string login, string password);

        Task<IActionResult> RefreshAsync(string accessToken, string refreshToken);

        Task<string> GetRoleAsync(string login);

        Task<object> GetCurrentUserAsync(HttpRequest request);
    }
}
