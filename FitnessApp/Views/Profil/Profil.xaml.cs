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
    //Autor: NiE

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
            GetFitFeed();
            BindingContext = ProfilVM;
            SetButton();
        }

        private void GetFitFeed()
        {
            var cache = AllVM.Datenbank.Feed.GetByUser(ProfilVM.User);
            if (cache != null)
                ProfilVM.FitFeed = cache.OrderByDescending(o => o.ErstelltAm).ToList();
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
                directChat.IsVisible = false;
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
                this.Navigation.PushAsync(new TrainingList(ProfilVM.User));
            }
            else if (button.ClassId == "EP")
            {
                this.Navigation.PushAsync(new MahlzeitList(ProfilVM.User));
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
                ProfilVM.FitFeed.First(s => s.ID.ToString() == id).LikedTimer = true;
                ProfilVM.FitFeed.First(s => s.ID.ToString() == id).Liked = true;
                ProfilVM.FitFeed.First(s => s.ID.ToString() == id).Likes += 1;
                timer.Start();
            }
            else if (result == false)
            {
                ProfilVM.FitFeed.First(s => s.ID.ToString() == id).Liked = false;
                ProfilVM.FitFeed.First(s => s.ID.ToString() == id).Likes -= 1;
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Liken");
        }

        private void DisableLikeImage(object sender, ElapsedEventArgs e)
        {
            ProfilVM.FitFeed.Find(s => s.ID.ToString() == idCache).LikedTimer = false;
            idCache = null;
        }

        void DirectChat(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("Baustelle", "Das ausgewählte Feature steht noch nicht zur Verfügung", "OK");
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            MenuItem menuItem = new MenuItem();
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell theViewCell = ((ViewCell)sender);
            News item = theViewCell.BindingContext as News;
            theViewCell.ContextActions.Clear();

            if (item != null)
            {
                if (item.Ersteller.Nutzername == AllVM.User.Nutzername)
                {
                    menuItem = new MenuItem()
                    {
                        Text = "Löschen",
                        ClassId = $"{item.ID}",
                        IsDestructive = true
                    };
                    menuItem.Clicked += Delete;
                    theViewCell.ContextActions.Add(menuItem);
                }
            }
        }

        async void Delete(object sender, EventArgs e)
        {
            MenuItem item = (sender as MenuItem);
            News news = ProfilVM.FitFeed.First(s => s.ID.ToString() == item.ClassId);

            if (await DisplayAlert("Löschen?", "Willst du den Post wirklich löschen?", "Ja", "Nein"))
            {
                if (AllVM.Datenbank.Feed.Delete(news))
                {
                    GetFitFeed();
                    DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gelöscht");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Fehler beim Löschen");
                }
            }
        }
    }
}