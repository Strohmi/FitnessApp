using System;
namespace FitnessApp
{
    public class User : NotifyPropertyBase
    {
        private string _nutzername;
        private string _passwort;
        private DateTime _erstelltAm;
        private string _infoText;
        private string _status;
        private byte[] _profilbild;

        public string Nutzername { get { return _nutzername; } set { OnPropertyChanged(ref _nutzername, value); } }
        public string InfoText { get { return _infoText; } set { OnPropertyChanged(ref _infoText, value); } }
        public string Status { get { return _status; } set { OnPropertyChanged(ref _status, value); } }
        public DateTime ErstelltAm { get { return _erstelltAm; } set { OnPropertyChanged(ref _erstelltAm, value); } }
        public string Passwort { get { return _passwort; } set { OnPropertyChanged(ref _passwort, value); } }
        public byte[] ProfilBild { get { return _profilbild; } set { OnPropertyChanged(ref _profilbild, value); } }
    }

}
