using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LessPaper.APIGateway.Helper
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates an email address
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>True if the address is valid</returns>
        public static bool IsValidEmailAddress(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && new EmailAddressAttribute().IsValid(email);
        }

    }
}
