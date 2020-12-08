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
using System.Reflection;
using System.Collections.ObjectModel;

namespace FitnessApp
{
    public partial class FitFeed : ContentPage
    {
        public FitFeedVM FitFeedVM { get; set; }
        public List<News> CacheList { get; set; }
        private Timer timer;
        private string idCache;
        private int days = 7;
        private int multiplikator = 1;
        private DateTime vonDatum;
        private DateTime bisDatum;

        public FitFeed()
        {
            FitFeedVM = new FitFeedVM();
            InitializeComponent();
            Start();
            BindingContext = FitFeedVM;
        }

        private void Start()
        {
            NavigationPage.SetIconColor(this, Color.White);
            vonDatum = DateTime.Now.AddDays(-days * multiplikator);
            GetList();

            if (FitFeedVM.ListNews.Count == 0)
            {
                FitFeedVM.ListNews.Add(new News()
                {
                    ID = -1,
                    Beschreibung = "Hey neuer Fitness-User!\n" +
                    "Ich finde es super, dass du die App ausprobierst!\n\n" +
                    "Wenn du Fragen hast, kannst du mich jederzeit gerne anschreiben!\n" +
                    "Bis dahin, wünsche ich dir viel Spaß beim Teilen deiner Fitnessaktivitäten, deinen leckeren Ernährungsplänen oder deinen anstregenden aber guten Trainingsplänen!\n\n" +
                    "Falls du neue Leute kennen lernen willst, die das gleiche Interesse wie du haben, geh einfach auf die Suche und finde neue Leute!\n\n" +
                    "Achja, und wenn du ein paar Fehler findest, schreib mir einfach. Ich leite die Nachricht ans Entwicklerteam weiter ;)\n\n" +
                    "Tschüssikowski und bis denne dein Fitness_Bot",
                    Ersteller = AllVM.Datenbank.User.GetByName("fitness_bot"),
                    ErstelltAm = DateTime.Now
                });
            }
        }

        void Loaded(System.Object sender, System.EventArgs e)
        {
            timer = new Timer()
            {
                Interval = 1000,
                AutoReset = false
            };
            timer.Elapsed += DisableLikeImage;
        }

        private void DisableLikeImage(object sender, ElapsedEventArgs e)
        {
            FitFeedVM.ListNews.First(s => s.ID.ToString() == idCache).LikedTimer = false;
            idCache = null;
        }

        void Refresh(System.Object sender, System.EventArgs e)
        {
            GetList();
            (sender as ListView).IsRefreshing = false;
        }

        private void GetList()
        {
            CacheList = AllVM.Datenbank.Feed.Get(AllVM.ConvertToUser(), vonDatum);

            if (CacheList != null)
            {
                FitFeedVM.ListNews.Clear();
                CacheList = CacheList.OrderByDescending(o => o.ErstelltAm).ToList();
                foreach (var item in CacheList)
                    FitFeedVM.ListNews.Add(item);
            }
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
                FitFeedVM.ListNews.First(s => s.ID.ToString() == id).LikedTimer = true;
                FitFeedVM.ListNews.First(s => s.ID.ToString() == id).Liked = true;
                FitFeedVM.ListNews.First(s => s.ID.ToString() == id).Likes += 1;
                timer.Start();
            }
            else if (result == false)
            {
                FitFeedVM.ListNews.First(s => s.ID.ToString() == id).Liked = false;
                FitFeedVM.ListNews.First(s => s.ID.ToString() == id).Likes -= 1;
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

        async void Delete(object sender, EventArgs e)
        {
            MenuItem item = (sender as MenuItem);
            News news = FitFeedVM.ListNews.First(s => s.ID.ToString() == item.ClassId);

            if (await DisplayAlert("Löschen?", "Willst du den Post wirklich löschen?", "Ja", "Nein"))
            {
                if (AllVM.Datenbank.Feed.Delete(news))
                {
                    GetList();
                    DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gelöscht");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Fehler beim Löschen");
                }
            }
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

        void LoadMore(System.Object sender, System.EventArgs e)
        {
            loadMoreBtn.IsVisible = false;
            vonDatum = DateTime.Now.AddDays(-days * (multiplikator + 1));
            bisDatum = DateTime.Now.AddDays(-days * multiplikator);
            CacheList = AllVM.Datenbank.Feed.Get(AllVM.ConvertToUser(), vonDatum, bisDatum);

            if (CacheList != null && CacheList.Count > 0)
            {
                int lastIndex = FitFeedVM.ListNews.Count;
                CacheList = CacheList.OrderByDescending(o => o.ErstelltAm).ToList();
                foreach (var item in CacheList)
                    FitFeedVM.ListNews.Add(item);

                multiplikator++;
                listview.ScrollTo(FitFeedVM.ListNews[lastIndex], ScrollToPosition.Start, true);
            }
            else if (CacheList.Count == 0)
                DependencyService.Get<IMessage>().ShortAlert("Keine weiteren Daten verfügbar");
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Abruf der Liste");
        }

        void ItemAppearing(System.Object sender, EventArgs e)
        {
            if (FitFeedVM.ListNews.Last() == ((sender as ViewCell).View.BindingContext as News))
            {
                loadMoreBtn.IsVisible = true;
            }
            else
            {
                loadMoreBtn.IsVisible = false;
            }
        }

        void ShowLikes(System.Object sender, System.EventArgs e)
        {
            StackLayout stack = (sender as StackLayout);

            if (FitFeedVM.ListNews.First(s => s.ID.ToString() == stack.ClassId).Likes != 0)
                this.Navigation.PushAsync(new Likes(stack.ClassId));
        }
    }
}