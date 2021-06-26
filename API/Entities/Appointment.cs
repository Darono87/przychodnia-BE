// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Appointment
    {
        [Key] public int Id { get; set; }

        [Required] public string Description { get; set; }

        public string Diagnosis { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public AppointmentStatus Status { get; set; }

        [Required] public DateTime ScheduledDate { get; set; }

        [Required] public DateTime RegistrationDate { get; set; }

        public DateTime? FinishDate { get; set; }

        [Required] public Doctor Doctor { get; set; }

        [Required] public Patient Patient { get; set; }

        [Required] public Registrar Registrar { get; set; }

        public ICollection<PhysicalExamination> PhysicalExaminations { get; set; }
        public ICollection<LabExamination> LabExaminations { get; set; }
    }

    public enum AppointmentStatus
    {
        Scheduled, Finished, Cancelled
    }
}
