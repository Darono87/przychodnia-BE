using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.DTO;

namespace API.Repositories
{
    public interface IPhysicalExaminationRepository
    {
        Task<PhysicalExamination> AddAsync(PhysicalExamination physicalExamination);
        Task<PhysicalExamination> UpdateAsync(PhysicalExamination physicalExamination);
        Task<PhysicalExamination> GetAsync(int id);
        Task<PaginationDTO<PhysicalExamination>> GetAllAsync(int[] appointments, int page, int perPage, bool isAscending, string sortKey);
    }
}
