// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class RegistrarRepository : IGenericUserRepository<Registrar>
    {
        private readonly DataContext context;

        public RegistrarRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add(Registrar obj)
        {
            context.Registrars.Add(obj);
            context.SaveChanges();
        }

        public void Update(Registrar obj)
        {
            context.Registrars.Update(obj);
            context.SaveChanges();
        }

        public Registrar Get(int id)
        {
            return context.Registrars.Find(id);
        }

        public Registrar Get(string login)
        {
            return context.Registrars
                .FirstOrDefault(a =>
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
