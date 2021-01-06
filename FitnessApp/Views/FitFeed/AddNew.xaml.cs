using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    //Autor: Niklas Erichsen

    public partial class AddNew : ContentPage
    {
        public AddNew()
        {
            InitializeComponent();
            Start();
        }

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        private void Start()
        {
            Title = "Neues hinzufügen";
        }

        /// <summary>
        /// Springen zur Seite Neuer Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GoToStatus(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new StatusNew());
        }

        /// <summary>
        /// Springen zur Seite Neuer Trainingsplan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GoToTrainingsplan(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new TrainingNew());
        }

        /// <summary>
        /// Springen zur Seite Neuer Ernährungsplan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GoToErnährunsplan(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new MahlzeitNew());
        }
    }
}