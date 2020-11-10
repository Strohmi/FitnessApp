using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class TrainingNew : ContentPage
    {
        public TrainingVM TrainingVM { get; set; }

        public TrainingNew()
        {
            InitializeComponent();
            TrainingVM = new TrainingVM();
            Start();
            BindingContext = TrainingVM;
        }

        private void Start()
        {
            Title = "Hinzufügen";
        }
    }
}