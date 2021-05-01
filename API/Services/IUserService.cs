// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Http;

namespace API.Services
{
    public interface IUserService
    {
        void Create(string role, string login, string firstName, string lastName, string password, string? permitNumber);

        AuthenticationDTO Authenticate(string login, string password);

        string GetRole(string login);

        object GetCurrentUser(HttpRequest request);
    }
}
