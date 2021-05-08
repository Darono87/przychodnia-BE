// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace API.Repositories
{
    public interface IGenericUserRepository<T>
    {
        Task<T> AddAsync(T obj);
        Task<T> UpdateAsync(T obj);
        Task<T> GetAsync(int id);
        Task<T> GetAsync(string login);
    }
}
