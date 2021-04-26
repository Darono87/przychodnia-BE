// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class DoctorRepository : IGenericUserRepository<Doctor>
    {
        private readonly DataContext context;

        public DoctorRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add(Doctor obj)
        {
            context.Doctors.Add(obj);
            context.SaveChanges();
        }

        public void Update(Doctor obj)
        {
            context.Doctors.Update(obj);
            context.SaveChanges();
        }

        public Doctor Get(int id)
        {
            return context.Doctors.Find(id);
        }

        public Doctor Get(string login)
        {
            return context.Doctors
                .FirstOrDefault(a => 
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
