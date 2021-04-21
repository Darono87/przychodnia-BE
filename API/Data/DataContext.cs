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
        public DbSet<Address> Addresses { get; set; }
    }
}
