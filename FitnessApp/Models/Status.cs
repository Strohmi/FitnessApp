using System;

namespace FitnessApp
{
    public class Status : NotifyPropertyBase
    {
        private string _title;
        private string _beschreibung;
        private byte[] _fotoBytes;

        public string Title { get { return _title; } set { OnPropertyChanged(ref _title, value); } }
        public string Beschreibung { get { return _beschreibung; } set { OnPropertyChanged(ref _beschreibung, value); } }
        public byte[] FotoBytes { get { return _fotoBytes; } set { OnPropertyChanged(ref _fotoBytes, value); } }
    }
}
