using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    //Autor: NiE

    public partial class AddNew : ContentPage
    {
        public AddNew()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            Title = "Neues hinzufügen";
        }

        void GoToStatus(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new StatusNew());
        }

        void GoToTrainingsplan(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new TrainingNew());
        }

        void GoToErnährunsplan(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new MahlzeitNew());
        }
    }
}