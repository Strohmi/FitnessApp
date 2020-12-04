using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp.Models
{
    public class Trainingsplan
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public User User { get; set; }
        public DateTime ErstelltAm { get; set; }
        public DateTime GeAendertAm { get; set; }
        public List<Uebung> UebungList { get; set; }
        public List<BewertungTrainingpsplan> Bewertungen { get; set; }
        public BewertungTrainingpsplan Bewertung { get; set; }
        public string Kategorie { get; set; }

        public decimal DurchBewertung { get; set; }
        public string DurchBewertAnzeige { get { return $"{DurchBewertung:0.00} / 5"; } }
    }

    //uebung
    public class Uebung
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Gewicht { get; set; }
        public int Repetition { get; set; }
        public int Sets { get; set; }

    }

    public class BewertungTrainingpsplan
    {
        public int ID { get; set; }
        public User Bewerter { get; set; }
        public string Bewertung { get; set; }
    }
}
