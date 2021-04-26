// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using API.DTO;

namespace API.Utils
{
    public interface IJwtManager
    {
        AuthenticationDTO GenerateTokens(string username, string role, DateTime startDate);
    }
}
