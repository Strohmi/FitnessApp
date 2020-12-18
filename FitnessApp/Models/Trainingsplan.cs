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
        public List<Uebung> UebungList { get; set; } = new List<Uebung>();
        public List<Bewertung> Bewertungen { get; set; } = new List<Bewertung>();
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
                else if (DurchBewertung == -2)
                {
                    return $"FEHLER";
                }
                else
                {
                    return $"{DurchBewertung:0.00} / 5";
                }
            }
        }
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
