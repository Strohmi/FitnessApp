using System;
namespace FitnessApp
{
    public class News : NotifyPropertyBase
    {
        public string Beschreibung { get; set; }

        public DateTime ErstelltAm { get; set; }
        public User Ersteller { get; set; }

        public byte[] Foto { get; set; }
        public char PlanArt { get; set; }
        public string PlanID { get; set; }
    }
}
