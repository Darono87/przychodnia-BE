// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class AdminRepository : IGenericUserRepository<Admin>
    {
        private readonly DataContext context;

        public AdminRepository(DataContext context)
        {
            this.context = context;
        }
        
        public void Add(Admin obj)
        {
            context.Admins.Add(obj);
            context.SaveChanges();
        }

        public void Update(Admin obj)
        {
            context.Admins.Update(obj);
            context.SaveChanges();
        }

        public Admin Get(int id)
        {
            return context.Admins.Find(id);
        }

        public Admin Get(string login)
        {
            return context.Admins
                .FirstOrDefault(a => 
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
