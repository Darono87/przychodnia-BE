using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface IExaminationCodeRepository
    {
        Task<ExaminationCode> AddAsync(ExaminationCode examinationCode);
        Task<ExaminationCode> UpdateAsync(ExaminationCode examinationCode);
        Task<ExaminationCode> GetAsync(int id);
        Task<ExaminationCode> GetAsync(string abbreviation);
    }
}
