using System;

namespace FitnessApp
{
    //Autor: NiE

    public class Status : NotifyPropertyBase
    {
        private string _beschreibung;
        private byte[] _foto;
        private User _erstelltVon;
        private DateTime _erstelltAm;

        public string Beschreibung { get { return _beschreibung; } set { OnPropertyChanged(ref _beschreibung, value); } }
        public byte[] Foto { get { return _foto; } set { OnPropertyChanged(ref _foto, value); } }
        public User ErstelltVon { get { return _erstelltVon; } set { OnPropertyChanged(ref _erstelltVon, value); } }
        public DateTime ErstelltAm { get { return _erstelltAm; } set { OnPropertyChanged(ref _erstelltAm, value); } }
    }
}
