using System;

namespace FitnessApp
{
    public class ProfilVM : NotifyPropertyBase
    {
        private User _user;
        public User User { get { return _user; } set { OnPropertyChanged(ref _user, value); } }
        public DateTime MinDate { get { return new DateTime(1900, 01, 01); } }
        public DateTime MaxDate { get { return DateTime.Now.Date.AddYears(-10); } } //Man muss mindestens 10 Jahre alt sein

        public ProfilVM()
        {
            User = new User()
            {
                Vorname = "Niklas",
                Nachname = "Erichsen",
                Geburtsdatum = new DateTime(1996, 02, 09)
            };
        }
    }
}
