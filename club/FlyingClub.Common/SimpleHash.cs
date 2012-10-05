using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace FlyingClub.Common
{
    public class SimpleHash
    {
        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        public static string MD5(string password, string salt)
        {
            string passwordHash = MD5(password);
            return MD5(String.Concat(passwordHash, salt));
        }

        public static string GetSalt(int length)
        {
            //Create and populate random byte array
            byte[] randomArray = new byte[length];
            string randomString;

            //Create random salt and convert to string
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            randomString = Convert.ToBase64String(randomArray);

            return randomString;
        }
    }
}
