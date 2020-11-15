using System;
using System.Text;

namespace FitnessApp.Models.General
{
    public class AllVM
    {
        public static User User { get; set; } = new User();
        public static Datenbank Datenbank { get; set; } = new Datenbank();

        public static string HashPassword(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2").ToLower());
                }
                return sb.ToString();
            }
        }
    }
}
