using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp.Models
{
    public class Bewertung
    {
        public int ID { get; set; }
        public int Rating { get; set; }
        public User Bewerter { get; set; }
    }
}
