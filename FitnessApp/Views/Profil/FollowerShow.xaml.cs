using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace FitnessApp
{
    public partial class FollowerShow : ContentPage
    {
        public User user { get; set; }
        public List<Follower> Follows { get; set; }

        public FollowerShow(User _user)
        {
            InitializeComponent();
            user = _user;
            Start();
            BindingContext = this;
        }

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        private void Start()
        {
            Title = "Follower";
            var users = AllVM.Datenbank.User.GetFollows(user.Nutzername);
            if (users != null)
            {
                Follows = users.OrderBy(s => s.User.Nutzername).ThenBy(o => o.GefolgtAm).ToList();
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
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }
    }
}