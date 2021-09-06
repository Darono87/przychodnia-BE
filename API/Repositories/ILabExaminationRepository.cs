using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.DTO;

namespace API.Repositories
{
    public interface ILabExaminationRepository
    {
        Task<LabExamination> AddAsync(LabExamination physicalExamination);
        Task<LabExamination> UpdateAsync(LabExamination physicalExamination);
        Task<LabExamination> GetAsync(int id);
        Task<PaginationDTO<LabExamination>> GetAllAsync(int[] appointments, ExaminationStatus[] statuses, int page, int perPage, bool isAscending, string sortKey);
    }
}
