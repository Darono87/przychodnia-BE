using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class PhysicalExaminationRepository : IPhysicalExaminationRepository
    {
        private readonly DataContext context;

        public PhysicalExaminationRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<PhysicalExamination> AddAsync(PhysicalExamination physicalExamination)
        {
            var result = await context.PhysicalExaminations.AddAsync(physicalExamination);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<PhysicalExamination> UpdateAsync(PhysicalExamination physicalExamination)
        {
            var result = context.PhysicalExaminations.Update(physicalExamination);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<PhysicalExamination> GetAsync(int id)
        {
            return await context.PhysicalExaminations.FindAsync(id);
        }

        public async Task<IEnumerable<PhysicalExamination>> GetAllAsync(Appointment appointment)
        {
            return await Task.FromResult(context.PhysicalExaminations.Include(c => c.Appointment)
                .Where(a => a.Appointment.Id == appointment.Id));
        }
    }
}
