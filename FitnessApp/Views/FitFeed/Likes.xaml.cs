using System.Collections.Generic;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;
using Xamarin.Forms;

namespace FitnessApp
{
    //Autor: Niklas Erichsen

    public partial class Likes : ContentPage
    {
        public string ID { get; set; }
        public List<Like> ListLikes { get; set; }

        public Likes(string _id)
        {
            InitializeComponent();
            ID = _id;
            Start();
            BindingContext = this;
        }

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        private void Start()
        {
            Title = "Likes";
            var users = AllVM.Datenbank.Feed.GetLikesWithNames(ID);
            if (users != null)
            {
                ListLikes = users.OrderBy(s => s.User.Nutzername).ToList();
                ListLikes.Find(s => s.User.Nutzername == AllVM.User.Nutzername).IsUser = true;
            }
            else
            {
                OnBackButtonPressed();
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Laden");
            }
        }

        /// <summary>
        /// Seite aus Stack löschen
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }

        /// <summary>
        /// Zum Profil springen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GoToProfil(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            Like like = (sender as ListView).SelectedItem as Like;
            if (like != null)
                this.Navigation.PushAsync(new Profil(like.User.Nutzername));
        }

        /// <summary>
        /// Abonnieren des Nutzers ohne auf das Profil gehen zu müssen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FollowOrUnFollow(System.Object sender, System.EventArgs e)
        {
            Button button = (sender as Button);

            if (button != null && !string.IsNullOrWhiteSpace(button.Text))
            {
                Like like = (button.Parent.Parent as ViewCell).BindingContext as Like;

                if (button.Text == "Entfolgen")
                {
                    AllVM.Datenbank.User.UnFollow(new User() { Nutzername = button.ClassId }, AllVM.ConvertToUser());
                    ListLikes[ListLikes.IndexOf(like)].IsSub = false;
                }
                else
                {
                    AllVM.Datenbank.User.Follow(new User() { Nutzername = button.ClassId }, AllVM.ConvertToUser());
                    ListLikes[ListLikes.IndexOf(like)].IsSub = true;
                }

                //Aktualisierung der Liste
                listView.ItemsSource = null;
                listView.ItemsSource = ListLikes;
            }
        }
    }
}