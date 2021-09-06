// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class RefreshToken
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(600)] public string Token { get; set; }
        [Required] public DateTime ValidTill { get; set; }
    }
}
