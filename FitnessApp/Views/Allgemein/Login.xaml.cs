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
        public List<string> Nutzernamen { get; set; }
        public string Nutzername { get; set; }
        public string Passwort { get; set; }

        private string hashedpw;

        public Login()
        {
            InitializeComponent();
            var cache = AllVM.Datenbank.User.GetList();
            if (cache != null)
                Nutzernamen = cache.Select(s => s.Nutzername).ToList();

            BindingContext = this;
        }

        private void Save(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Nutzername))
            {
                User user = AllVM.Datenbank.User.GetByName(Nutzername);
                AllVM.User = AllVM.ConvertFromUser(user);
                App.Current.MainPage = new AppShell();
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Nutzername eingeben!");
            }
        }

        private void Registrate(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Registrate()); //Öffnet die neue Seite Registrate

            //this.Navigation.PopAsync(); Terminiert die aktuelle Seite
        }

        private void Help(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Help());
        }

        private void Loading()
        {
            anmelden.IsEnabled = false;
            //loading.IsVisible = true;
            //loading.IsRunning = true;
        }
    }
}