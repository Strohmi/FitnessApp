using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.Models;
using FitnessApp.Models.General;
using FitnessApp.Models.DB;
using System.Linq;

namespace FitnessApp
{
    public partial class TrainingsplanAnsicht : ContentPage
    {
        public List<Ernährungsplan> EPlaene { get; set; }
        private User user;

        public TrainingsplanAnsicht(User _user)
        {
            InitializeComponent();
            user = _user;
            Start();
            BindingContext = this;
        }

        void Loaded(System.Object sender, System.EventArgs e)
        {
            GetList();
        }

        void Start()
        {
            Title = "Ernährungspläne";
        }

        void GetList()
        {
          //Not Implementet   
        }

        
    }
}
