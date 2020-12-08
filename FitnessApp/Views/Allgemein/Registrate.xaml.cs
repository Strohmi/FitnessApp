using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;
using System.Text;
using FitnessApp.ViewModels;
using System.Net;

namespace FitnessApp
{
    public partial class Registrate : ContentPage
    {
        public RegisterVM RegVM { get; set; } = new RegisterVM();

        public Registrate()
        {
            InitializeComponent();
            OnStart();
            BindingContext = RegVM;
        }

        private void OnStart()
        {
            Title = "Registrieren";
            NavigationPage.SetBackButtonTitle(this, "Zurück");
            NavigationPage.SetHasBackButton(this, true);
        }

        async void Registrieren(System.Object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RegVM.User.Nutzername))
            {
                RegVM.User.Nutzername = RegVM.User.Nutzername.ToLower();
                if (!string.IsNullOrWhiteSpace(RegVM.PW))
                {
                    if (!string.IsNullOrWhiteSpace(RegVM.PW2))
                    {
                        if (RegVM.PW == RegVM.PW2)
                        {
                            string hashedpw = AllVM.HashPassword(RegVM.PW);

                            RegVM.User.ErstelltAm = DateTime.Now;
                            RegVM.User.Passwort = hashedpw;
                            RegVM.User.InfoText = "Hi, ich bin neu hier!";

                            using (var webClient = new WebClient())
                                RegVM.User.ProfilBild = webClient.DownloadData("https://cdn.pixabay.com/photo/2016/11/11/09/59/white-male-1816195_1280.jpg");

                            if (AllVM.Datenbank.User.Insert(RegVM.User))
                            {
                                AllVM.User = AllVM.ConvertFromUser(RegVM.User);
                                Application.Current.Properties.Add("userid", RegVM.User.Nutzername);
                                Application.Current.Properties.Add("userpw", RegVM.User.Passwort);
                                await Application.Current.SavePropertiesAsync();

                                App.Current.MainPage = new AppShell();
                            }
                            else
                                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern!");
                        }
                        else
                            DependencyService.Get<IMessage>().ShortAlert("Passwörter stimmen nicht überein");
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Bitte Bestätigung eingeben");
                }
                else
                    DependencyService.Get<IMessage>().ShortAlert("Bitte Passwort eingeben");
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Nutzername eingeben!");
        }
    }
}