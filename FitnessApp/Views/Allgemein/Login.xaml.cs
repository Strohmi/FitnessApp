using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;

namespace FitnessApp
{
    public partial class Login : ContentPage
    {

        public string Nutzername { get; set; }
        public string Passwort { get; set; }

        private string hashedpw;

        public Login()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void Save(object sender, EventArgs e)
        {
            _ = Nutzername;
        }

    }
}