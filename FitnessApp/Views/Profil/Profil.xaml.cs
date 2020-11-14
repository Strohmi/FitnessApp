using System;
using System.Collections.Generic;
using System.ComponentModel;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class Profil : ContentPage
    {
        private User userCache;
        public ProfilVM ProfilVM { get; set; }

        public Profil()
        {
            InitializeComponent();
            userCache = null;
            Start();
        }

        public Profil(User user)
        {
            InitializeComponent();
            userCache = AllVM.User;
            Start();
        }

        private void Loaded(object sender, EventArgs e)
        {
            User user = null;
            if (userCache == null)
                user = AllVM.User;
            else
                user = userCache;
            ProfilVM = new ProfilVM(user);
            BindingContext = ProfilVM;
            SetNavBar();
        }

        private void Start()
        {
            Title = "Profil";
        }

        private void SetNavBar()
        {
            ToolbarItems.Clear();
            if (ProfilVM.User.Nutzername == AllVM.User.Nutzername)
            {
                ToolbarItem item = new ToolbarItem
                {
                    Text = "Bearbeiten",
                    Order = ToolbarItemOrder.Primary,
                    Priority = 0
                };
                item.Clicked += Edit;
                ToolbarItems.Add(item);
            }
            //else
            //{
            List<User> follows = AllVM.Datenbank.User.GetFollows(ProfilVM.User.Nutzername);
            if (follows != null)
            {
                if (follows.Exists(s => s.Nutzername == AllVM.User.Nutzername))
                    ProfilVM.AboBtnText = "Entfolgen";
                else
                    ProfilVM.AboBtnText = "Folgen";
            }
            //}
        }

        void Follow_UnFollow(System.Object sender, System.EventArgs e)
        {
            Button button = (sender as Button);

            if (button.Text == "Folgen")
                Follow();
            else if (button.Text == "Entfolgen")
                UnFollow();
        }

        private void Follow()
        {
            if (AllVM.Datenbank.User.Follow(ProfilVM.User, AllVM.User))
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
            if (AllVM.Datenbank.User.UnFollow(ProfilVM.User, AllVM.User))
            {
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich entfolgt!");
                ProfilVM.AboBtnText = "Folgen";
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern");
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            DependencyService.Get<IMessage>().ShortAlert("Bearbeiten geht noch nicht");
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
    }
}