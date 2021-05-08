// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

#nullable enable
namespace API.DTO
{
    public class AuthenticationDTO
    {
        public AuthenticationDTO()
        {
            AccessToken = "";
            RefreshToken = "";
            Role = "";
        }

        public AuthenticationDTO(string accessToken, string refreshToken, string role)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Role = role;
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                null => false,
                AuthenticationDTO other => AccessToken == other.AccessToken && RefreshToken == other.RefreshToken &&
                                           Role == other.Role,
                _ => base.Equals(obj)
            };
        }

        protected bool Equals(AuthenticationDTO other)
        {
            return AccessToken == other.AccessToken && RefreshToken == other.RefreshToken && Role == other.Role;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccessToken, RefreshToken, Role);
        }
    }
}
