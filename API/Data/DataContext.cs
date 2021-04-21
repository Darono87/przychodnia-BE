using System.Diagnostics.CodeAnalysis;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{

    public class DataContext : DbContext
    {
        public DataContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Registrator> Registrators { get; set; }
        public DbSet<LabTechnician> LabTechnicians { get; set; }
        public DbSet<LabManager> LabManagers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PhysicalExamination> PhysicalExaminations { get; set; }
        public DbSet<LabExamination> LabExaminations { get; set; }
        public DbSet<ExaminationStatus> ExaminationStatuses { get; set; }
        public DbSet<ExaminationCode> ExaminationCodes { get; set; }
        public DbSet<ExaminationType> ExaminationTypes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
