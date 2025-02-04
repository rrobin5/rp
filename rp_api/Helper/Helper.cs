﻿using System.Security.Cryptography;
using System.Text;

namespace rp_api.Helper
{
    public class Helper : IHelper
    {
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
