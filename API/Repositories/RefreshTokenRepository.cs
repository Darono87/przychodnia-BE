// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DataContext context;

        public RefreshTokenRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<RefreshToken> Get(string token)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(refreshToken => refreshToken.Token == token);
        }

        public async Task<RefreshToken> Add(RefreshToken refreshToken)
        {
            var result = await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<RefreshToken> Update(RefreshToken refreshToken)
        {
            var result = context.RefreshTokens.Update(refreshToken);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task Remove(string token)
        {
            var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

            if (refreshToken != null)
            {
                context.RefreshTokens.Remove(refreshToken);
                await context.SaveChangesAsync();
            }
        }
    }
}
