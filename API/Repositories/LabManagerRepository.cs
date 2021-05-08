// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.Linq;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class LabManagerRepository : IGenericUserRepository<LabManager>
    {
        private readonly DataContext context;

        public LabManagerRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add(LabManager obj)
        {
            context.LabManagers.Add(obj);
            context.SaveChanges();
        }

        public void Update(LabManager obj)
        {
            context.LabManagers.Update(obj);
            context.SaveChanges();
        }

        public LabManager Get(int id)
        {
            return context.LabManagers.Find(id);
        }

        public LabManager Get(string login)
        {
            return context.LabManagers
                .FirstOrDefault(a =>
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
