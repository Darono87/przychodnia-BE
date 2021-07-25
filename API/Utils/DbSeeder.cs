using System.Collections.Generic;
using System.Linq;
using API.Data;
using API.Entities;

namespace API.Utils
{
    public class DbSeeder
    {
        private readonly DataContext context;

        public DbSeeder(DataContext context)
        {
            this.context = context;
        }

        public void SeedData()
        {
            if (context.Users.Any() || context.Patients.Any() || context.ExaminationCodes.Any()) return;

            context.Users.AddRange(PrepareUsers());
            context.SaveChanges();
            context.Admins.AddRange(PrepareAdmins());
            context.Registrars.AddRange(PrepareRegistrars());
            context.ExaminationCodes.AddRange(PrepareExaminationCodes());
            context.Patients.AddRange(PreparePatients());
            context.SaveChanges();
        }

        private static IEnumerable<User> PrepareUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    FirstName = "Abc",
                    LastName = "Def",
                    PasswordHash = "$2b$10$pATLDI3YYbmqD/..bdZn9uIUi0duq7.y2VAFn2G2BavrosC9A41gG",
                    Login = "Admin123"
                },
                new User
                {
                    FirstName = "Regi",
                    LastName = "Strar",
                    PasswordHash = "$2b$10$JCg3NGAf93FZdLPafqemWO2j1IGBJGpITIgLI5s8ltWZuFiRj3YLS",
                    Login = "Regist123"
                }
            };

            return users;
        }

        private IEnumerable<Admin> PrepareAdmins()
        {
            var admins = new List<Admin> {new Admin {User = context.Users.Find(1)}};

            return admins;
        }

        private IEnumerable<Registrar> PrepareRegistrars()
        {
            var registrars = new List<Registrar> {new Registrar {User = context.Users.Find(2)}};

            return registrars;
        }

        private static IEnumerable<ExaminationCode> PrepareExaminationCodes()
        {
            var examinationCodes = new List<ExaminationCode>
            {
                new ExaminationCode
                {
                    Abbreviation = "RBC", Name = "Erythrocytes", Type = ExaminationType.Laboratory
                },
                new ExaminationCode {Abbreviation = "MCH", Name = "Hemoglobin", Type = ExaminationType.Laboratory},
                new ExaminationCode {Abbreviation = "WBC", Name = "Leukocytes", Type = ExaminationType.Laboratory},
                new ExaminationCode
                {
                    Abbreviation = "OVR", Name = "Overall well-being", Type = ExaminationType.Physical
                },
                new ExaminationCode
                {
                    Abbreviation = "PSY", Name = "Psychological state", Type = ExaminationType.Physical
                },
                new ExaminationCode
                {
                    Abbreviation = "ROM", Name = "Romberg's trial", Type = ExaminationType.Physical
                },
                new ExaminationCode {Abbreviation = "BCB", Name = "Backbone", Type = ExaminationType.Physical},
                new ExaminationCode {Abbreviation = "CHS", Name = "Chest", Type = ExaminationType.Physical},
                new ExaminationCode {Abbreviation = "STM", Name = "Stomach", Type = ExaminationType.Physical}
            };

            return examinationCodes;
        }

        private static IEnumerable<Patient> PreparePatients()
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    PeselNumber = "59092367874",
                    Address = new Address
                    {
                        Country = "Poland",
                        City = "Warsaw",
                        PostalCode = "00-001",
                        Street = "Grunwaldzka",
                        BuildingNumber = "10"
                    }
                },
                new Patient
                {
                    FirstName = "Adam",
                    LastName = "Nowak",
                    PeselNumber = "63051265161",
                    Address = new Address
                    {
                        Country = "Poland",
                        City = "Warsaw",
                        PostalCode = "00-001",
                        Street = "Grunwaldzka",
                        BuildingNumber = "10"
                    }
                }
            };

            return patients;
        }
    }
}
