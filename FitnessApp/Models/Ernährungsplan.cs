using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp.Models
{
    public class Ernährungsplan
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public User Ersteller { get; set; }
        public DateTime ErstelltAm { get; set; }
        public DateTime GeAendertAm { get; set; }
        public List<Mahlzeiten> MahlzeitenList { get; set; } = new List<Mahlzeiten>();
        public List<Bewertung> Bewertungen { get; set; } = new List<Bewertung>();
        public string Kategorie { get; set; }

        public decimal DurchBewertung { get; set; }
    }

    public class Mahlzeiten
    {
        public int ID { get; set; }
        public string Nahrungsmittel { get; set; }
        public decimal Menge { get; set; }
        public string Einheit { get; set; }
    }
}
