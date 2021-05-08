// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class LabManager
    {
        [ForeignKey("UserId")] public User User { get; set; }

        [Key] public int Id { get; set; }

        public ICollection<LabExamination> Examinations { get; set; }
    }
}
