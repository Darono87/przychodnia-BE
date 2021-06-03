using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories
{
    public interface IPhysicalExaminationRepository
    {
        Task<PhysicalExamination> AddAsync(PhysicalExamination physicalExamination);
        Task<PhysicalExamination> UpdateAsync(PhysicalExamination physicalExamination);
        Task<PhysicalExamination> GetAsync(int id);
        Task<IEnumerable<PhysicalExamination>> GetAllAsync(Appointment appointment);
    }
}
