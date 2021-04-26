// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class RegistratorRepository : IGenericUserRepository<Registrator>
    {
        private readonly DataContext context;

        public RegistratorRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add(Registrator obj)
        {
            context.Registrators.Add(obj);
            context.SaveChanges();
        }

        public void Update(Registrator obj)
        {
            context.Registrators.Update(obj);
            context.SaveChanges();
        }

        public Registrator Get(int id)
        {
            return context.Registrators.Find(id);
        }

        public Registrator Get(string login)
        {
            return context.Registrators
                .FirstOrDefault(a => 
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
