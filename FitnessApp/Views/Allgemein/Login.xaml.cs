using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;

namespace FitnessApp
{
    public partial class Login : ContentPage
    {
        public string[] Nutzernamen
        {
            get
            {
                return new string[] { "nikeri", "tuneke", "timbru", "nicmis", "lasstr" };
            }
        }
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
            if (!string.IsNullOrWhiteSpace(Nutzername))
            {
                AllVM.User = AllVM.Datenbank.User.GetByName(Nutzername);
                App.Current.MainPage = new AppShell();
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Nutzername eingeben!");
            }
        }
    }
}