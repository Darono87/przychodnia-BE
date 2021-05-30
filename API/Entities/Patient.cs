// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Patient
    {
        [Key] public int Id { get; set; }

        [Required] [MaxLength(80)] public string FirstName { get; set; }

        [Required] [MaxLength(160)] public string LastName { get; set; }

        [Required] [MaxLength(11)] public string PeselNumber { get; set; }

        [Required] public Address Address { get; set; }

        public ICollection<Appointment> Appointments;
    }
}
