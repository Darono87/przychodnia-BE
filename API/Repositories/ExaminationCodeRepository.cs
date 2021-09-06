using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public async Task<SuggestionsDto> GetAllAsync(ExaminationType examinationType)
        {
            var codes = await context.ExaminationCodes
                .Where(code=>code.Type == examinationType)
                .Select(code=>new SuggestionsDto.Suggestion(){
                    label = code.Abbreviation + ": " + code.Name,
                    value = code.Id
                }).ToArrayAsync();

            return new SuggestionsDto(){Suggestions=codes};
        }
    }
}
