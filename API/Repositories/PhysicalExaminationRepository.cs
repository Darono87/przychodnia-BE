using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
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

        public async Task<PaginationDTO<PhysicalExamination>> GetAllAsync(int[] appointments, int page, int perPage, bool isAscending, string sortKey)
        {
            var currentPage = page > 0 ? page : 1;
            var itemCount = perPage > 0 ? perPage : 20;

            var physicalContext = context.PhysicalExaminations
                .Where(ex=>appointments.Contains(ex.Appointment.Id))
                .Include(ex => ex.Appointment);

            System.Func<PhysicalExamination, object> orderFun = sortKey switch{
                "examinationCode" =>  (PhysicalExamination a)=>a.ExaminationCode,
                "appointmentId" => (PhysicalExamination a) =>a.Appointment.Id,
                "doctor" => (PhysicalExamination a) => a.Appointment.Doctor.User.FirstName,
                _ => (PhysicalExamination a) =>a.Result // "result"
            };

            var physicalSorted = physicalContext.OrderBy(orderFun);
            if(!isAscending) {
                physicalSorted = physicalContext.OrderByDescending(orderFun);
                if(sortKey == "doctor")
                    physicalSorted = physicalSorted.ThenByDescending(a=>a.Appointment.Doctor.User.LastName);
            } else{
                if(sortKey == "doctor")
                    physicalSorted = physicalSorted.ThenBy(a=>a.Appointment.Doctor.User.LastName);
            }

            var ph = await Task.FromResult(
                physicalSorted
                .Skip(itemCount * (currentPage - 1))
                .Take(itemCount)
                .AsEnumerable());

            var count = await Task.FromResult(context.PhysicalExaminations
                .Where(ex=>appointments.Contains(ex.Appointment.Id))
                .Count());

            return new PaginationDTO<PhysicalExamination>{items=ph, count=count};
        }
    }
}
