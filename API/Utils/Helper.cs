// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace API.Utils
{
    public class Helper
    {
        public static bool ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            return true;
        }
    }
}
