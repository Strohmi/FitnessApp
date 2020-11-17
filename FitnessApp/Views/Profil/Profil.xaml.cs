using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class Profil : ContentPage
    {
        public ProfilShowVM ProfilVM { get; set; }
        private User user;
        private bool isOther;

        public Profil()
        {
            InitializeComponent();
            isOther = false;
            Start();
        }

        public Profil(User _user, bool _isOther = true)
        {
            InitializeComponent();
            user = AllVM.Datenbank.User.GetByName(_user.Nutzername);
            isOther = _isOther;
            Start();
        }

        private void Loaded(object sender, EventArgs e)
        {
            if (isOther == false)
                user = AllVM.ConvertToUser();
            ProfilVM = new ProfilShowVM(user);
            ProfilVM.FitFeed = AllVM.Datenbank.Feed.GetByUser(ProfilVM.User);

            //Entfernen nach Auto-Login !
            if (ProfilVM.User.ProfilBild == null)
                using (var webClient = new WebClient())
                {
                    ProfilVM.User.ProfilBild = webClient.DownloadData("https://cdn.pixabay.com/photo/2016/11/11/09/59/white-male-1816195_1280.jpg");
                }

            BindingContext = ProfilVM;

            //SetBeispiele();
            SetButton();
        }

        private void SetBeispiele()
        {
            ProfilVM.FitFeed = new List<News>()
            {
              new News()
              {
                  Beschreibung = "Heute geiles Training gehabt!",
                  ErstelltAm = new DateTime(2019,12,31),
                  Ersteller = ProfilVM.User
              },
              new News()
              {
                  Beschreibung = "Leuties, ich hab nen neuen Trainingsplan.\nCheckt den mal aus !!!1!!!1!",
                  ErstelltAm = new DateTime(2020,11,11),
                  Ersteller = ProfilVM.User
              },
            }.OrderByDescending(o => o.ErstelltAm).ToList();
        }

        private void Start()
        {
            Title = "Profil";
        }

        private void SetButton()
        {
            if (ProfilVM.User.Nutzername == AllVM.User.Nutzername)
            {
                ProfilVM.AboBtnText = "Bearbeiten";
            }
            else
            {
                List<User> follows = AllVM.Datenbank.User.GetFollows(ProfilVM.User.Nutzername);
                if (follows != null)
                {
                    if (follows.Exists(s => s.Nutzername == AllVM.User.Nutzername))
                        ProfilVM.AboBtnText = "Entfolgen";
                    else
                        ProfilVM.AboBtnText = "Folgen";
                }
            }
        }

        void Follow_UnFollow(System.Object sender, System.EventArgs e)
        {
            Button button = (sender as Button);

            if (button.Text == "Folgen")
                Follow();
            else if (button.Text == "Entfolgen")
                UnFollow();
            else if (button.Text == "Bearbeiten")
                Edit();
        }

        private void Follow()
        {
            if (AllVM.Datenbank.User.Follow(ProfilVM.User, AllVM.ConvertToUser()))
            {
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gefolgt!");
                ProfilVM.AboBtnText = "Entfolgen";
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern");
            }
        }

        private void UnFollow()
        {
            if (AllVM.Datenbank.User.UnFollow(ProfilVM.User, AllVM.ConvertToUser()))
            {
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich entfolgt!");
                ProfilVM.AboBtnText = "Folgen";
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern");
            }
        }

        private void Edit()
        {
            user = null;
            this.Navigation.PushAsync(new ProfilEdit());
        }

        void GoToPlan(System.Object sender, System.EventArgs e)
        {
            Button button = (sender as Button);

            if (button.ClassId == "TP")
            {
                DisplayAlert("Plan", "Hier sehen Sie die Trainingspläne", "OK");
            }
            else if (button.ClassId == "EP")
            {
                DisplayAlert("Plan", "Hier sehen Sie die Ernährungspläne", "OK");
            }
        }

        void ProfilBildTapped(System.Object sender, System.EventArgs e)
        {
            profilBildBig.IsVisible = true;
            grid.IsVisible = false;
        }

        void BigProfilBildTapped(System.Object sender, System.EventArgs e)
        {
            profilBildBig.IsVisible = false;
            grid.IsVisible = true;
        }
    }
}