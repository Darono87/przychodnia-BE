// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class LabTechnicianRepository : IGenericUserRepository<LabTechnician>
    {
        private readonly DataContext context;

        public LabTechnicianRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add(LabTechnician obj)
        {
            context.LabTechnicians.Add(obj);
            context.SaveChanges();
        }

        public void Update(LabTechnician obj)
        {
            context.LabTechnicians.Update(obj);
            context.SaveChanges();
        }

        public LabTechnician Get(int id)
        {
            return context.LabTechnicians.Find(id);
        }

        public LabTechnician Get(string login)
        {
            return context.LabTechnicians
                .FirstOrDefault(a =>
                    a.User.Id == context.Users
                        .FirstOrDefault(u => u.Login == login).Id);
        }
    }
}
