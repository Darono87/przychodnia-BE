// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Http;

namespace API.Services
{
    public interface IUserService
    {
        Task<User> Create(string role, string login, string firstName, string lastName, string password,
            string? permitNumber);

        Task<AuthenticationDTO> Authenticate(string login, string password);

        Task<AuthenticationDTO> Refresh(string accessToken, string refreshToken);

        Task<string> GetRole(string login);

        Task<object> GetCurrentUser(HttpRequest request);
    }
}
