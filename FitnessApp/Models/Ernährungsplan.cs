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
        public List<Mahlzeiten> MahlzeitenList { get; set; } = new List<Mahlzeiten>();
        public List<BewertungErnährungsplan> Bewertungen { get; set; } = new List<BewertungErnährungsplan>();
        public string Kategorie { get; set; }

        public decimal DurchBewertung { get; set; }
        public string DurchBewertAnzeige
        {
            get
            {
                if (DurchBewertung == -1)
                {
                    return "???";
                }
                else if(DurchBewertung == -2)
                {
                    return $"FEHLER";
                }
                else
                {
                    return $"{DurchBewertung:00.0} / 5";
                }
            }
        }
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
