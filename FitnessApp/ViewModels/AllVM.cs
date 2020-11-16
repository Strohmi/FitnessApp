using System;
using System.Text;

namespace FitnessApp.Models.General
{
    public class AllVM
    {
        public static User2 User { get; set; } = new User2();
        public static Datenbank Datenbank { get; set; } = new Datenbank();

        public static User2 ConvertFromUser(User _user)
        {
            return new User2()
            {
                Nutzername = _user.Nutzername,
                InfoText = _user.InfoText,
                Passwort = _user.Passwort,
                ProfilBild = _user.ProfilBild,
                ErstelltAm = _user.ErstelltAm,
                AnzahlFollower = _user.AnzahlFollower
            };
        }

        public static User ConvertToUser()
        {
            return new User()
            {
                Nutzername = User.Nutzername,
                InfoText = User.InfoText,
                Passwort = User.Passwort,
                ProfilBild = User.ProfilBild,
                ErstelltAm = User.ErstelltAm,
                AnzahlFollower = User.AnzahlFollower
            };
        }

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
