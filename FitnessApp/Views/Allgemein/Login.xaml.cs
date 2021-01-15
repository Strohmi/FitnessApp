using System;
using Xamarin.Forms;

namespace FitnessApp
{
    public partial class Login : ContentPage
    {
        public string Nutzername { get; set; }
        public string Passwort { get; set; }

        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this;
        }

        /// <summary>
        /// Anmeldedaten prüfen, lokal speichern und an den Server melden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Save(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Nutzername))
            {
                Nutzername = Nutzername.ToLower();
                bool? exists = AllVM.Datenbank.User.Exists(Nutzername);

                if (exists == true)
                {
                    string pw = AllVM.Datenbank.User.GetPasswort(Nutzername);
                    string hpw = AllVM.HashPassword(Passwort);
                    if (hpw == pw)
                    {
                        AllVM.Initial(Nutzername);
                        Application.Current.Properties.Clear();
                        Application.Current.Properties.Add("userid", Nutzername);
                        Application.Current.Properties.Add("userpw", pw);
                        await Application.Current.SavePropertiesAsync();
                        App.Current.MainPage = new AppShell();
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Passwort stimmt nicht!");
                }
                else if (exists == false)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Nutzername unbekannt");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Fehler beim Prüfen");
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Nutzername eingeben!");
            }
        }

        /// <summary>
        /// Zur Seite Registrierung springen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Registrate(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Registrate());
        }

        /// <summary>
        /// Zur Seite Help springen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Help());
        }
    }
}