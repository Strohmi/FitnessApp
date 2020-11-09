using System;

namespace FitnessApp
{
    public class Status : NotifyPropertyBase
    {
        private string _title;
        private string _beschreibung;

        public string Title { get { return _title; } set { OnPropertyChanged(ref _title, value); } }
        public string Beschreibung { get { return _beschreibung; } set { OnPropertyChanged(ref _beschreibung, value); } }
    }
}
