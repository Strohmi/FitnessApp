using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;
using System.Text;

namespace FitnessApp
{
    public partial class Registrate : ContentPage
    {
        public string[] Nutzernamen
        {
            get
            {
                return new string[] { "nikeri", "tuneke", "timbru", "nicmis", "lasstr" };
            }
        }
        public string NewNutzername { get; set; }
        public string NewPasswort { get; set; }
        public string NewPasswort2 { get; set; }

        public Registrate()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void NewUser(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewNutzername))
            {
                if (!string.IsNullOrWhiteSpace(NewPasswort))
                {
                    if (!string.IsNullOrWhiteSpace(NewPasswort2))
                    {
                        if (NewPasswort == NewPasswort2)
                        {
                            string hashedpw = AllVM.HashPassword(NewPasswort);

                            User user = new User()
                            {
                                Nutzername = NewNutzername,
                                Passwort = hashedpw,
                                ErstelltAm = DateTime.Now
                            };

                            if (AllVM.Datenbank.User.Insert(user))
                            {
                                AllVM.User = AllVM.ConvertFromUser(user);
                                Application.Current.Properties.Add("userid", user.Nutzername);
                                await Application.Current.SavePropertiesAsync();
                            }

                            else
                                DependencyService.Get<IMessage>().ShortAlert("Fehler junge!");
                        }
                        else
                            DependencyService.Get<IMessage>().ShortAlert("Passwörter stimmen nicht überein");
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Bitte Passwort eingeben");
                }
                else
                    DependencyService.Get<IMessage>().ShortAlert("Bitte Passwort eingeben");
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Nutzername eingeben!");
        }


    }
}