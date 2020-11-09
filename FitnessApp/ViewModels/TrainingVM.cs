using System;
using System.Collections.Generic;

namespace FitnessApp
{
    public class TrainingVM
    {
        public List<string> Options { get; set; }

        public TrainingVM()
        {
            AddOptions();
        }

        private void AddOptions()
        {
            Options = new List<string>();
            Options.Add("Status");
            Options.Add("Trainingsplan");
            Options.Add("Ernährungsplan");
        }
    }
}
