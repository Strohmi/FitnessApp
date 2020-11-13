using System;
namespace FitnessApp
{
    public class User : NotifyPropertyBase
    {
        private string _nutzername;
        private string _passwort;
        private byte[] _profilbild;

        public string Nutzername { get { return _nutzername; } set { OnPropertyChanged(ref _nutzername, value); } }
        public string Passwort { get { return _passwort; } set { OnPropertyChanged(ref _passwort, value); } }
        public byte[] ProfilBild { get { return _profilbild; } set { OnPropertyChanged(ref _profilbild, value); } }
    }
}
