using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ExaminationCodeRepository : IExaminationCodeRepository
    {
        private readonly DataContext context;

        public ExaminationCodeRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<ExaminationCode> AddAsync(ExaminationCode examinationCode)
        {
            var result = await context.ExaminationCodes.AddAsync(examinationCode);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<ExaminationCode> UpdateAsync(ExaminationCode examinationCode)
        {
            var result = context.ExaminationCodes.Update(examinationCode);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<ExaminationCode> GetAsync(int id)
        {
            return await context.ExaminationCodes.FindAsync(id);
        }

        public async Task<ExaminationCode> GetAsync(string abbreviation)
        {
            return await context.ExaminationCodes.FirstOrDefaultAsync(ec => ec.Abbreviation == abbreviation);
        }
    }
}
