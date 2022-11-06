using System;
using System.Security.Cryptography;
using System.Text;

namespace EBSAuthenticationHandler.Helpers
{
    public static class PasswordHelper
    {
        public static string EncodePassword(string password)
        {
            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);

            return Convert.ToBase64String(pwdBytes);
        }
    }
}

