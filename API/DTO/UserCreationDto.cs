// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using API.Utils;

namespace API.DTO
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "Role is required")]
        [Role]
        public string Role { get; set; }
        
        [Required(ErrorMessage = "Login is required")]
        [MaxLength(100)]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(80)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(160)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [Password]
        public string Password { get; set; }
        
        public string PermitNumber { get; set; }
    }
}
