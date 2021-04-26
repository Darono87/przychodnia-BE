// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTO;
using Microsoft.IdentityModel.Tokens;

namespace API.Utils
{
    public class JwtManager : IJwtManager
    {
        private readonly ConcurrentDictionary<string, string> refreshTokens;
        private readonly JwtConfig jwtConfig;
        private readonly byte[] secret;

        public JwtManager(JwtConfig jwtConfig)
        {
            this.jwtConfig = jwtConfig;

            refreshTokens = new ConcurrentDictionary<string, string>();
            secret = Encoding.ASCII.GetBytes(this.jwtConfig.Secret);
        }

        public AuthenticationDTO GenerateTokens(string username, string role, DateTime startDate)
        {
            var accessToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                new Claim[] { new(ClaimTypes.Role, role) },
                expires: startDate.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
                );
            var refreshToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                new Claim[] { new(ClaimTypes.Role, role) },
                expires: startDate.AddMinutes(jwtConfig.RefreshTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            );

            var result = new AuthenticationDTO()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken), 
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken), 
                Role = role
            };

            refreshTokens.AddOrUpdate(result.RefreshToken, result.RefreshToken, (key, value) => result.RefreshToken);

            return result;
        }
    }
}
