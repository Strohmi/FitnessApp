using System;
using Xamarin.Forms;
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

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        private void OnStart()
        {
            Title = "Registrieren";
            NavigationPage.SetBackButtonTitle(this, "Zurück");
            NavigationPage.SetHasBackButton(this, true);
        }

        /// <summary>
        /// Prüfen der Daten für die Registrierung und dann speichern der Daten lokal wie in der Datenbank
        /// Voreinstellung einer Benutzerdaten, sodass ein bessere Userfeeling entsteht
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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