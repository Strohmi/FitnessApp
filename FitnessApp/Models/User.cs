using System;

namespace FitnessApp
{
    /// <summary>
    /// Benutzerklasse
    /// </summary>
    public class User : NotifyPropertyBase
    {
        private string _nutzername;
        private string _customName;
        private string _passwort;
        private DateTime _erstelltAm;
        private string _infoText;
        private byte[] _profilbild;
        private int _anzahlFollower;
        private bool _onlyCustomName;

        public string Nutzername { get { return _nutzername; } set { OnPropertyChanged(ref _nutzername, value); } }
        public string CustomName { get { return _customName; } set { OnPropertyChanged(ref _customName, value); } }
        public string InfoText { get { return _infoText; } set { OnPropertyChanged(ref _infoText, value); } }
        public DateTime ErstelltAm { get { return _erstelltAm; } set { OnPropertyChanged(ref _erstelltAm, value); } }
        public string Passwort { get { return _passwort; } set { OnPropertyChanged(ref _passwort, value); } }
        public byte[] ProfilBild { get { return _profilbild; } set { OnPropertyChanged(ref _profilbild, value); } }
        public int AnzahlFollower { get { return _anzahlFollower; } set { OnPropertyChanged(ref _anzahlFollower, value); } }
        public bool OnlyCustomName { get { return _onlyCustomName; } set { OnPropertyChanged(ref _onlyCustomName, value); } }
    }

    public class User2
    {
        public string Nutzername { get; set; }
        public string CustomName { get; set; }
        public string InfoText { get; set; }
        public string Passwort { get; set; }
        public byte[] ProfilBild { get; set; }
        public DateTime ErstelltAm { get; set; }
        public int AnzahlFollower { get; set; }
        public bool OnlyCustomName { get; set; }
    }
}
