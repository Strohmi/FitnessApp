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
    //Autor: NiE

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

            if (AllVM.Datenbank.User.GetSubs(AllVM.User.Nutzername).Count == 0)
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
                    Ersteller = new User() { Nutzername = "fitness_bot" },
                    ErstelltAm = DateTime.Now
                });
            }
            else
            {
                vonDatum = DateTime.Now.AddDays(-days * multiplikator);
                GetList();

                while (FitFeedVM.ListNews != null & FitFeedVM.ListNews.Count == 0)
                {
                    vonDatum = DateTime.Now.AddDays(-days * multiplikator);
                    GetList();
                }
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
                multiplikator++;
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Abruf der Liste");
        }

        void Like(System.Object sender, System.EventArgs e)
        {
            string id = (sender as Frame).ClassId;
            Liken(id);
        }

        void Liken(string id)
        {
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
            ZumProfil(label.ClassId);
        }

        void ZumProfil(string nutzername)
        {
            if (nutzername == AllVM.User.Nutzername)
                this.Navigation.PushAsync(new Profil());
            else
                this.Navigation.PushAsync(new Profil(nutzername));
        }

        async void Delete(string id)
        {
            News news = FitFeedVM.ListNews.First(s => s.ID.ToString() == id);

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

        void LoadMore(object sender, EventArgs e)
        {
            loadMoreBtn.IsVisible = false;
            vonDatum = DateTime.Now.AddDays(-days * (multiplikator + 1)).AddDays(1);
            bisDatum = DateTime.Now.AddDays(-days * multiplikator);
            CacheList = AllVM.Datenbank.Feed.Get(AllVM.ConvertToUser(), vonDatum, bisDatum);

            if (CacheList == null)
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Abruf der Liste");
            else
            {
                multiplikator++;
                foreach (var item in CacheList)
                    FitFeedVM.ListNews.Add(item);
            }

            while (CacheList != null && CacheList.Count == 0 && vonDatum > DateTime.Now.AddMonths(-3))
            {
                vonDatum = DateTime.Now.AddDays(-days * (multiplikator + 1)).AddDays(1);
                bisDatum = DateTime.Now.AddDays(-days * multiplikator);
                CacheList = AllVM.Datenbank.Feed.Get(AllVM.ConvertToUser(), vonDatum, bisDatum);

                CacheList = CacheList.OrderByDescending(o => o.ErstelltAm).ToList();
                foreach (var item in CacheList)
                    FitFeedVM.ListNews.Add(item);

                multiplikator++;
            }
        }

        void ItemAppearing(System.Object sender, EventArgs e)
        {
            if (((sender as ViewCell).View.BindingContext as News) == FitFeedVM.ListNews.Last())
                loadMoreBtn.IsVisible = true;
        }

        void ShowLikes(System.Object sender, System.EventArgs e)
        {
            StackLayout stack = (sender as StackLayout);
            News news = FitFeedVM.ListNews.First(s => s.ID.ToString() == stack.ClassId);

            if (news.Likes != 0)
                this.Navigation.PushAsync(new Likes(stack.ClassId));
        }

        async void GoToSettings(System.Object sender, System.EventArgs e)
        {
            News news = FitFeedVM.ListNews.First(s => s.ID.ToString() == (sender as Image).ClassId);

            List<string> auswahl = new List<string>();

            if (news.Liked)
                auswahl.Add("Gefällt mir nicht mehr");
            else
                auswahl.Add("Gefällt mir");

            auswahl.Add("Zum Profil");

            if (AllVM.User.Nutzername == news.Ersteller.Nutzername)
                auswahl.Add("Löschen");

            var result = await DisplayActionSheet("Einstellungen", "Abbruch", null, auswahl.ToArray());

            switch (result)
            {
                case "Gefällt mir":
                case "Gefällt mir nicht mehr":
                    Liken(news.ID.ToString());
                    break;
                case "Zum Profil":
                    ZumProfil(news.Ersteller.Nutzername);
                    break;
                case "Löschen":
                    Delete(news.ID.ToString());
                    break;
                default:
                    break;
            }
        }
    }
}