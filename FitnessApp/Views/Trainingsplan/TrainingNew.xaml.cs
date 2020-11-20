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
            Start();
            BindingContext = TrainingVM;
        }

        public TrainingVM TrainingVM { get; set; }
        public string Uebungen { get; set; }
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        public decimal Weights { get; set; }

        private void Start()
        {
            Title = "Trainingsplan erstellen";
        }

        private void Add(object sender, EventArgs e)
        {

        }

        // Liest die Entry Zeilen aus und Fügt sie der Entsprechenden Liste hinzu
        private void ReadEntry()
        {

              
        }
        private void Save(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}