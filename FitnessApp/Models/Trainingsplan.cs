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
        public string Bewertung { get; set; }
        public DateTime ErstelltAm { get; set; }
        public DateTime GeAendertAm { get; set; }
        public List<Uebung> UebungList { get; set; }
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
}
