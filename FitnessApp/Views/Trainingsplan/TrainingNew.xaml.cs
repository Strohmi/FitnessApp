using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class TrainingNew : ContentPage
    {
        public TrainingNew()
        {
            InitializeComponent();
            TrainingVM = new TrainingVM();
            Uebungen = new List<string>();
            Repetitions = new List<int>();
            Sets = new List<int>();
            Weights = new List<decimal>();
            Start();
            BindingContext = TrainingVM;
        }

        public TrainingVM TrainingVM { get; set; } 
        public List<string> Uebungen { get; set; }
        public List<int> Repetitions { get; set; }
        public List<int> Sets { get; set; }
        public List<decimal> Weights { get; set; }

        private void Start()
        {
            Title = "Trainingsplan erstellen";
        }

        private void Add(object sender, EventArgs e)
        {
            
        }
    }
}