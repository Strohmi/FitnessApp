using System;
namespace FitnessApp
{
    public class User : NotifyPropertyBase
    {
        private string _vorname;
        private string _nachname;
        private string _passwort;
        private DateTime _geburtsdatum;

        public string Vorname { get { return _vorname; } set { OnPropertyChanged(ref _vorname, value); } }
        public string Nachname { get { return _nachname; } set { OnPropertyChanged(ref _nachname, value); } }
        public string Passwort { get { return _passwort; } set { OnPropertyChanged(ref _passwort, value); } }
        public DateTime Geburtsdatum { get { return _geburtsdatum; } set { OnPropertyChanged(ref _geburtsdatum, value); } }

        public string VollName { get { return Vorname + " " + Nachname; } }
    }
}
