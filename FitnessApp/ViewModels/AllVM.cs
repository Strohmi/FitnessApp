using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FitnessApp.Models.General
{
    public class AllVM
    {
        public static User2 User { get; set; } = new User2();
        public static Datenbank Datenbank { get; set; } = new Datenbank();

        /// <summary>
        /// Umwandlung von User mit automatischer BindingFunktion zu User ohne BindingFunktion
        /// </summary>
        /// <param name="_user">Benutzer</param>
        /// <returns></returns>
        public static User2 ConvertFromUser(User _user)
        {
            return new User2()
            {
                Nutzername = _user.Nutzername,
                InfoText = _user.InfoText,
                Passwort = _user.Passwort,
                ProfilBild = _user.ProfilBild,
                ErstelltAm = _user.ErstelltAm,
                AnzahlFollower = _user.AnzahlFollower,
                OnlyCustomName = _user.OnlyCustomName,
                CustomName = _user.CustomName
            };
        }

        /// <summary>
        /// Umwandlung von User ohne automatische BindingFunktion zu User fürs Binding
        /// </summary>
        /// <returns></returns>
        public static User ConvertToUser()
        {
            return new User()
            {
                Nutzername = User.Nutzername,
                InfoText = User.InfoText,
                Passwort = User.Passwort,
                ProfilBild = User.ProfilBild,
                ErstelltAm = User.ErstelltAm,
                AnzahlFollower = User.AnzahlFollower,
                OnlyCustomName = User.OnlyCustomName,
                CustomName = User.CustomName
            };
        }

        /// <summary>
        /// Verschlüsseln des Passwortes mit MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Speicherung des Benutzers zum seitenübergreifenden Verwenden
        /// </summary>
        /// <param name="user"></param>
        internal static void Initial(string user)
        {
            User = ConvertFromUser(Datenbank.User.GetByName(user));
        }
    }
}
