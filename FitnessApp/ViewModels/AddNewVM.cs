using System;
using System.Collections.Generic;

namespace FitnessApp
{
    public class AddNewVM
    {
        public Dictionary<int, string> Options { get; set; }

        public AddNewVM()
        {
            AddOptions();
        }

        private void AddOptions()
        {
            Options = new Dictionary<int, string>();
            Options.Add(0, "Status");
            Options.Add(1, "Trainingsplan");
            Options.Add(2, "Ernährungsplan");
        }
    }
}
