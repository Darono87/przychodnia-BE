// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace API.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base(
            "Password must have at least 8 characters (min. 1 capital letter, 1 number)")
        {
        }
    }
}
