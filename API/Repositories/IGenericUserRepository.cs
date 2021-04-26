// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace API.Repositories
{
    public interface IGenericUserRepository<T>
    {
        void Add(T obj);
        void Update(T obj);
        T Get(int id);
        T Get(string login);
    }
}
