// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> Get(string token);
        Task<RefreshToken> Add(RefreshToken refreshToken);
        Task<RefreshToken> Update(RefreshToken refreshToken);
        Task Remove(string token);
    }
}
