using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class LabExaminationRepository : ILabExaminationRepository
    {
        private readonly DataContext context;

        public LabExaminationRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<LabExamination> AddAsync(LabExamination labExamination)
        {
            var result = await context.LabExaminations.AddAsync(labExamination);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<LabExamination> UpdateAsync(LabExamination labExamination)
        {
            var result = context.LabExaminations.Update(labExamination);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<LabExamination> GetAsync(int id)
        {
            return await context.LabExaminations.FindAsync(id);
        }

        public async Task<IEnumerable<LabExamination>> GetAllAsync(Appointment appointment, int page, int perPage, bool isAscending, string sortKey)
        {
            return await Task.FromResult(context.LabExaminations.Include(c => c.Appointment)
                .Where(a => a.Appointment.Id == appointment.Id));
        }
    }
}
