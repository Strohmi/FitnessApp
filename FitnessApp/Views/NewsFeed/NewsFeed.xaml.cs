using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;
using System.Threading.Tasks;
using System.Timers;

namespace FitnessApp
{
    public partial class NewsFeed : ContentPage
    {
        public NewsFeedVM NewsFeedVM { get; set; }
        private Timer timer;
        private string idCache;

        public NewsFeed()
        {
            InitializeComponent();
            NewsFeedVM = new NewsFeedVM();
            Start();
            BindingContext = NewsFeedVM;
        }

        private void Start()
        {
            NavigationPage.SetIconColor(this, Color.White);
        }

        void Loaded(System.Object sender, System.EventArgs e)
        {
            timer = new Timer()
            {
                Interval = 1000,
                AutoReset = false
            };
            timer.Elapsed += DisableLikeImage;

            GetList();
        }

        private void DisableLikeImage(object sender, ElapsedEventArgs e)
        {
            NewsFeedVM.ListNews.Find(s => s.ID.ToString() == idCache).LikedTimer = false;
            idCache = null;
        }

        void GoToSearch(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new ProfilSearch());
        }

        void GoToChats(System.Object sender, System.EventArgs e)
        {
        }

        void Refresh(System.Object sender, System.EventArgs e)
        {
            GetList();
            (sender as ListView).IsRefreshing = false;
        }

        private void GetList()
        {
            NewsFeedVM.ListNews = AllVM.Datenbank.Feed.Get(AllVM.ConvertToUser());
            if (NewsFeedVM.ListNews != null)
                NewsFeedVM.ListNews = NewsFeedVM.ListNews.OrderByDescending(o => o.ErstelltAm).ToList();
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Abruf der Liste");
        }

        void Like(System.Object sender, System.EventArgs e)
        {
            string id = (sender as Frame).ClassId;
            bool? result = AllVM.Datenbank.Feed.Like(id, AllVM.ConvertToUser());
            if (result == true)
            {
                idCache = id;
                NewsFeedVM.ListNews.Find(s => s.ID.ToString() == id).LikedTimer = true;
                NewsFeedVM.ListNews.Find(s => s.ID.ToString() == id).Liked = true;
                NewsFeedVM.ListNews.Find(s => s.ID.ToString() == id).Likes += 1;
                timer.Start();
            }
            else if (result == false)
            {
                NewsFeedVM.ListNews.Find(s => s.ID.ToString() == id).Liked = false;
                NewsFeedVM.ListNews.Find(s => s.ID.ToString() == id).Likes -= 1;
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Liken");
        }

        void GoToProfil(System.Object sender, System.EventArgs e)
        {
            Label label = (sender as Label);

            if (label.Text == AllVM.User.Nutzername)
                this.Navigation.PushAsync(new Profil());
            else
                this.Navigation.PushAsync(new Profil(label.Text));
        }

        async void Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            News news = NewsFeedVM.ListNews.Find(s => s.ID.ToString() == (sender as Frame).ClassId);

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    if (news.Ersteller.Nutzername == AllVM.User.Nutzername)
                    {
                        if (await DisplayAlert("Löschen?", "Willst du den Post wirklich löschen?", "Ja", "Nein"))
                            if (AllVM.Datenbank.Feed.Delete(news))
                                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gelöscht");
                            else
                                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Löschen");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}