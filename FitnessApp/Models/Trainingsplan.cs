using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp.Models
{
    public class Trainingsplan
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public User Ersteller { get; set; }
        public DateTime ErstelltAm { get; set; }
        public DateTime GeAendertAm { get; set; }
        public List<Uebung> UebungList { get; set; } = new List<Uebung>();
        public List<Bewertung> Bewertungen { get; set; } = new List<Bewertung>();
        public string Kategorie { get; set; }
        public decimal DurchBewertung { get; set; }
    }

    //Uebung
    public class Uebung
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Sätze { get; set; }
        public int Wiederholungen { get; set; }
        public decimal Menge { get; set; }
        public string Einheit { get; set; }
    }
}
