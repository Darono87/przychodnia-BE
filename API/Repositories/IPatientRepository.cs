using API.Entities;

namespace API.Repositories
{
    public interface IPatientRepository
    {
        void Add(Patient patient);
        void Update(Patient patient);
        Patient Get(int id);
        Patient Get(string PeselNumber);
    }
}
