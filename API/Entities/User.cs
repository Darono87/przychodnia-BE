// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required]
        [MaxLength(80)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(160)]
        public string LastName { get; set; }
        
        [Required]
        [MaxLength(400)]
        public string PasswordHash { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Registrator> Registrators { get; set; }
        public ICollection<LabTechnician> LabTechnicians { get; set; }
        public ICollection<LabManager> LabManagers { get; set; }
    }
}
