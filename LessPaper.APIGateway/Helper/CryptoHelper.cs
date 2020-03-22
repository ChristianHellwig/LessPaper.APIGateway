using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LessPaper.APIGateway.Helper
{
    public static class CryptoHelper
    {
        private static readonly RNGCryptoServiceProvider SecureRandomProvider = new RNGCryptoServiceProvider();

        /// <summary>
        /// Generates a Sha256 hash from the concatination of the given values
        /// </summary>
        /// <param name="value">Main value</param>
        /// <param name="salt">Random salt</param>
        /// <returns>Hash as Base64 format</returns>
        public static string Sha256FromString(string value, string salt)
        {
            using var sha = SHA256.Create();
            var saltedPassword = $"{salt}{value}";
            var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
            return Convert.ToBase64String(sha.ComputeHash(saltedPasswordAsBytes));
        }

        /// <summary>
        /// Generates a secure salt
        /// </summary>
        /// <param name="byteCount">Number of bytes</param>
        /// <returns>Salt</returns>
        public static string GetSalt(uint byteCount = 10)
        {
            var byteArray = new byte[byteCount];
            SecureRandomProvider.GetBytes(byteArray);
            return Convert.ToBase64String(byteArray);
        }

        /// <summary>
        /// Generates a new guid
        /// </summary>
        /// <returns>Guid string</returns>
        public static string GetGuid()
        {
            var guid = Guid.NewGuid();
            return guid.ToString();
        }

        //public static string GenerateJwt(IEnumerable<Claim> claims)
        //{

        //}


    }
}
