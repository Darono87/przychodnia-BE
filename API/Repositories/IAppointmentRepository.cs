using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.DTO;

namespace API.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<Appointment> GetAsync(int id);
        Task<PaginationDTO<Appointment>> GetAllAsync(int page, int perPage);

        Task<SuggestionsDto> GetSuggestionsAsync(int doctorId);

        Task<IEnumerable<Appointment>> GetAllFilteredAsync(int page, int perPage, string peselNumber,
            string permitNumber);
    }
}
