using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp.Models
{
    public class Ernährungsplan
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public User User { get; set; }
        public DateTime ErstelltAm { get; set; }
        public DateTime GeAendertAm { get; set; }
        public List<Mahlzeiten> MahlzeitenList { get; set; }
        public List<BewertungErnährungsplan> Bewertungen { get; set; }
        public string Kategorie { get; set; }
    }

    public class Mahlzeiten
    {
        public int ID { get; set; }
        public string Nahrungsmittel { get; set; }
        public decimal Menge { get; set; }
        public string Einheit { get; set; }
    }
    public class BewertungErnährungsplan
    {
        public int ID { get; set; }
        public string Bewertung { get; set; }
        public User Bewerter { get; set; }
    }
}
