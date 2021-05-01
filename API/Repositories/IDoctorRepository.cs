using API.Entities;

namespace API.Repositories
{
    public interface IDoctorRepository
    {
        public Doctor GetByPermitNumber(string permitNumber);
    }
}
