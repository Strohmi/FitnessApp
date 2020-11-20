using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class TrainingFavo : ContentPage
    {
        public TrainingFavo()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            Title = "Favoriten";
        }

        private void GetList()
        {

        }
    }
}