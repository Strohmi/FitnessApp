using System;
namespace FitnessApp
{
    public class News : NotifyPropertyBase
    {
        public string Title { get; set; }
        public DateTime ErstelltAm { get; set; }
        public User Ersteller { get; set; }
        public string Beschreibung { get; set; }
        public byte[] Foto { get; set; }
    }
}
