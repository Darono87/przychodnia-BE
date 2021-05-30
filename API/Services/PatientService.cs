// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        public async Task<IActionResult> RegisterAsync(PatientDto patientDto)
        {
            if (await patientRepository.GetAsync(patientDto.PeselNumber) != null)
            {
                return new JsonResult(new ExceptionDto {Message = "Patient with given identity number already exists"})
                {
                    StatusCode = 422
                };
            }

            var patient = new Patient
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                PeselNumber = patientDto.PeselNumber,
                Address = new Address
                {
                    Country = patientDto.Country,
                    City = patientDto.City,
                    BuildingNumber = patientDto.BuildingNumber,
                    PostalCode = patientDto.PostalCode,
                    Street = patientDto.Street
                }
            };

            return new JsonResult(await patientRepository.AddAsync(patient)) {StatusCode = 201};
        }

        public async Task<IActionResult> UpdateAsync(int id, PatientDto patientDto)
        {
            var patient = await patientRepository.GetAsync(id);

            if (patient == null)
            {
                return new JsonResult(new ExceptionDto {Message = "Could not find patient"}) {StatusCode = 422};
            }

            var patientWithPesel = await patientRepository.GetAsync(patientDto.PeselNumber);

            if (patientWithPesel != null && patientWithPesel != patient)
            {
                return new JsonResult(new ExceptionDto {Message = "Patient with given PESEL number already exists"})
                {
                    StatusCode = 422
                };
            }

            patient.FirstName = patientDto.FirstName;
            patient.LastName = patientDto.LastName;
            patient.Address.City = patientDto.City;
            patient.Address.Country = patientDto.Country;
            patient.Address.Street = patientDto.Street;
            patient.Address.BuildingNumber = patientDto.BuildingNumber;
            patient.Address.PostalCode = patientDto.PostalCode;

            return new JsonResult(await patientRepository.UpdateAsync(patient));
        }

        public async Task<IActionResult> GetAllAsync(int page, int perPage)
        {
            return new JsonResult(await patientRepository.GetAllAsync(page, perPage));
        }
    }
}
