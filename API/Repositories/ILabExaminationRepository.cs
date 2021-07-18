using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface ILabExaminationRepository
    {
        Task<LabExamination> AddAsync(LabExamination physicalExamination);
        Task<LabExamination> UpdateAsync(LabExamination physicalExamination);
        Task<LabExamination> GetAsync(int id);
        Task<IEnumerable<LabExamination>> GetAllAsync(Appointment appointment, int page, int perPage, bool isAscending, string sortKey);
    }
}
