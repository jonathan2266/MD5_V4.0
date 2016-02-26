using System;
using System.Security.Cryptography;
using System.Text;

namespace MD5_V2
{
    public class hasher
    {
        public string StartHash(string text)
        {
            byte[] tmpscrc = ASCIIEncoding.ASCII.GetBytes(text);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(tmpscrc);
            StringBuilder output = new StringBuilder();
            string solution;
            for (int i = 0; i < hash.Length; i++)
            {
                output.Append(hash[i].ToString("X2"));
            }
            solution = Convert.ToString(output);
            return solution;

        }

    }
}
