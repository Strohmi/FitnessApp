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
    }
}