using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.Database
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            var md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();

            foreach (byte res in result)
            {
                strBuilder.Append(res.ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
