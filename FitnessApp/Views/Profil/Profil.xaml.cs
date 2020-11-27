using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Timers;
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
        private Timer timer;
        private string idCache;

        public Profil()
        {
            InitializeComponent();
            isOther = false;
            Start();
        }

        public Profil(string _nutzername, bool _isOther = true)
        {
            InitializeComponent();
            user = AllVM.Datenbank.User.GetByName(_nutzername);
            isOther = _isOther;
            Start();
        }

        private void Loaded(object sender, EventArgs e)
        {
            if (isOther == false)
                user = AllVM.ConvertToUser();
            ProfilVM = new ProfilShowVM(user);
            var cache = AllVM.Datenbank.Feed.GetByUser(ProfilVM.User);
            if (cache != null)
                ProfilVM.FitFeed = cache.OrderByDescending(o => o.ErstelltAm).ToList();

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

        private void Start()
        {
            Title = "Profil";

            timer = new Timer()
            {
                Interval = 1000,
                AutoReset = false
            };
            timer.Elapsed += DisableLikeImage;
        }

        private void SetButton()
        {
            if (ProfilVM.User.Nutzername == AllVM.User.Nutzername)
            {
                ProfilVM.AboBtnText = "Bearbeiten";
            }
            else
            {
                List<Models.Follower> follows = AllVM.Datenbank.User.GetFollows(ProfilVM.User.Nutzername);
                if (follows != null)
                {
                    if (follows.Exists(s => s.User.Nutzername == AllVM.User.Nutzername))
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

        void Show_Follower(System.Object sender, System.EventArgs e)
        {
            if (ProfilVM.User.Nutzername == AllVM.User.Nutzername && ProfilVM.User.AnzahlFollower > 0)
            {
                this.Navigation.PushAsync(new Follower(ProfilVM.User));
            }
        }

        void Like(System.Object sender, System.EventArgs e)
        {
            string id = (sender as Frame).ClassId;
            bool? result = AllVM.Datenbank.Feed.Like(id, AllVM.ConvertToUser());
            if (result == true)
            {
                idCache = id;
                ProfilVM.FitFeed.Find(s => s.ID.ToString() == id).Liked = true;
                ProfilVM.FitFeed.Find(s => s.ID.ToString() == id).Likes += 1;
                timer.Start();
            }
            else if (result == false)
                ProfilVM.FitFeed.Find(s => s.ID.ToString() == id).Likes -= 1;
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Liken");
        }

        private void DisableLikeImage(object sender, ElapsedEventArgs e)
        {
            ProfilVM.FitFeed.Find(s => s.ID.ToString() == idCache).Liked = false;
            idCache = null;
        }
    }
}