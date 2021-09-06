// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class LabExamination
    {
        [Key] public int Id { get; set; }

        public string DoctorRemarks { get; set; }

        [Required] public DateTime IssueDate { get; set; }

        public string Result { get; set; }

        public DateTime? FinishDate { get; set; }

        public string ManagerRemarks { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public ExaminationStatus Status { get; set; }

        [Required] public ExaminationCode ExaminationCode { get; set; }

        public LabTechnician Technician { get; set; }
        public LabManager Manager { get; set; }

        [Required] public Appointment Appointment { get; set; }
    }

    public enum ExaminationStatus
    {
        Scheduled, Finished, Cancelled, CancelledByManager, Accepted
    }
}
