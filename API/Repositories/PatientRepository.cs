using System.Linq;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataContext context;

        public PatientRepository(DataContext context)
        {
            this.context = context;
        }
        
        public void Add(Patient patient)
        {
            context.Patients.Add(patient);
            context.SaveChanges();
        }

        public void Update(Patient patient)
        {
            context.Patients.Update(patient);
            context.SaveChanges();
        }

        public Patient Get(int id)
        {
            return context.Patients.Find(id);
        }

        public Patient Get(string PeselNumber)
        {
            return context.Patients
                .FirstOrDefault(a => 
                    a.PeselNumber == PeselNumber);
        }
    }
}
