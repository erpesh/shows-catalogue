using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    // Ref: https://stackoverflow.com/questions/4181198/how-to-hash-a-password
    class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int Iterations = 1000;
        private const int HashSize = 20;

        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create a password hash using PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Combine the salt and hash
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            // Extract the salt and hash from the stored password hash
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Compute a new hash of the input password using the stored salt
            var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, salt, Iterations);
            byte[] inputHash = pbkdf2.GetBytes(HashSize);

            // Compare the computed hash with the stored hash
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != inputHash[i])
                    return false;
            }

            return true;
        }
    }

}
