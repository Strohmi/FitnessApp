using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp
{
    public class Meal
    {
        public string Food { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return Amount + Unit + " " + Food;
        }
    }
}
