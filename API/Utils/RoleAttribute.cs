// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace API.Utils
{
    public class RoleAttribute : ValidationAttribute
    {
        private static readonly string[] roles = new string[] {"Admin","Registrar","Doctor","LabTechnician","LabManager"};
        public string GetErrorMessage() =>
            "Role has to be one of: Admin, Registrar, Doctor, LabTechnician or LabManager";


        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var role = (string)value;

            return roles.Any(role.Contains) ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
        }
    }
}
