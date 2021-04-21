// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class ExaminationCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ExaminationType Type { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Abbreviation { get; set; }

        public ICollection<PhysicalExamination> PhysicalExaminations { get; set; }
        public ICollection<LabExamination> LabExaminations { get; set; }
    }
}
