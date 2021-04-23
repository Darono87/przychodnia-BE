// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using API.Entities;
using API.Repositories;

namespace API.Services
{
    public interface IUserService
    { 
        void Create(string login, string firstName, string lastName, string password, string? permitNumber);
        
        void Authenticate(string login, string password);
    }
}
