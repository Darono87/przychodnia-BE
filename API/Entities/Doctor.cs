﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Doctor
    {
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Key] public int Id { get; set; }

        [Required] public string PermitNumber { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
