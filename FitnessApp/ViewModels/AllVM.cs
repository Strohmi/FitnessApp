using System;
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
    }
}
