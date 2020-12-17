using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    //Autor: NiE

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

        private void Start()
        {
            Title = "Likes";
            var users = AllVM.Datenbank.Feed.GetLikesWithNames(ID);
            if (users != null)
            {
                users.RemoveAll(s => s.User.Nutzername == AllVM.User.Nutzername);
                ListLikes = users.OrderBy(s => s.User.Nutzername).ToList();
            }
            else
            {
                OnBackButtonPressed();
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Laden");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }

        void GoToProfil(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            Like like = (sender as ListView).SelectedItem as Like;
            if (like != null)
                this.Navigation.PushAsync(new Profil(like.User.Nutzername));
        }

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
                listView.ItemsSource = null;
                listView.ItemsSource = ListLikes;
            }
        }
    }
}