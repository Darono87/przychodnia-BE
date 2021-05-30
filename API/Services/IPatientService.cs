// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IPatientService
    {
        Task<IActionResult> RegisterAsync(PatientDto patientDto);
        Task<IActionResult> UpdateAsync(int id, PatientDto patientDto);
        Task<IActionResult> GetAllAsync(int page, int perPage);
    }
}
