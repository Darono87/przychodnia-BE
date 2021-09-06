using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.DTO;
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

        public async Task<PaginationDTO<LabExamination>> GetAllAsync(int[] appointments, ExaminationStatus[] statuses, int page, int perPage, bool isAscending, string sortKey)
        {
            var currentPage = page > 0 ? page : 1;
            var itemCount = perPage > 0 ? perPage : 20;

            var labContext = context.LabExaminations
                .Where(ex=>(statuses.Length == 0 || statuses.Contains(ex.Status)) && (appointments.Length == 0 || appointments.Contains(ex.Appointment.Id)))
                .Include(ex => ex.Appointment)
                .Include(ex => ex.Manager)
                .Include(ex => ex.Technician)
                .Include(ex => ex.ExaminationCode);

            System.Func<LabExamination, object> orderFun = sortKey switch{
                "status" =>  (LabExamination a)=>a.Status,
                "doctorRemarks" => (LabExamination a) =>a.DoctorRemarks ,
                "issueDate" => (LabExamination a) =>a.IssueDate ,
                "result" => (LabExamination a) => a.Result,
                "finishDate" => (LabExamination a) => a.FinishDate,
                "confirmationDate" => (LabExamination a) => a.ConfirmationDate,
                "examinationCode" => (LabExamination a) => a.ExaminationCode.Name,
                "appointmentId" => (LabExamination a) => a.Appointment.Id,
                _ => (LabExamination a) => a.ManagerRemarks // "managerRemarks"
            };

            var labSorted = labContext.OrderBy(orderFun);
            if(!isAscending) {
                labSorted = labContext.OrderByDescending(orderFun);
            }

            var labs = await Task.FromResult(
                labSorted
                .Skip(itemCount * (currentPage - 1))
                .Take(itemCount)
                .AsEnumerable());

            var count = await Task.FromResult(context.LabExaminations
                .Where(ex=>(statuses.Length == 0 || statuses.Contains(ex.Status)) && (appointments.Length == 0 || appointments.Contains(ex.Appointment.Id)))
                .Count());

            return new PaginationDTO<LabExamination>{items=labs, count=count};
        }
    }
}
