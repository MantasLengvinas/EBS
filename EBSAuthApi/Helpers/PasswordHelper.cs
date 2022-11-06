using System;
using System.Security.Cryptography;
using System.Text;

namespace EBSAuthApi.Helpers
{
    public static class PasswordHelper
    {
        public static bool ValidatePassword(string password, string savedPassword)
        {
            byte[] pwdBytes = Convert.FromBase64String(password);
            password = Encoding.UTF8.GetString(pwdBytes);

            byte[] hashBytes = Convert.FromBase64String(savedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = deriveBytes.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }

        public static string HashPassword(string password)
        {
            byte[] pwdBytes = Convert.FromBase64String(password);
            password = Encoding.UTF8.GetString(pwdBytes);

            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt, 100000);

            byte[] hash = deriveBytes.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }
    }
}

