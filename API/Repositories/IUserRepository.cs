// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using API.Entities;

namespace API.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        User Get(int id);
        User Get(string login);
    }
}
