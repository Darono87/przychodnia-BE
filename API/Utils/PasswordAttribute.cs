// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace API.Utils
{
    public class PasswordAttribute : ValidationAttribute
    {
        private static readonly string[] capitalLetters = new string[]
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y", "Z"
        };

        private static readonly string[] numbers = new string[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};

        public string GetErrorMessage() =>
            "Password must have at least 8 characters with at least 1 capital letter and 1 number.";


        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var password = (string)value;

            if (password.Trim().Length < 8 || !numbers.Any(password.Contains) || !capitalLetters.Any(password.Contains))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
