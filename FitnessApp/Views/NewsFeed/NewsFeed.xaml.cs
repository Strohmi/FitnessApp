using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;

namespace FitnessApp
{
    public partial class NewsFeed : ContentPage
    {
        public NewsFeedVM NewsFeedVM { get; set; }

        public NewsFeed()
        {
            InitializeComponent();
            NewsFeedVM = new NewsFeedVM();
            Start();
            BindingContext = NewsFeedVM;
        }

        private void Start()
        {
            Title = "FitFeed";
            NavigationPage.SetIconColor(this, Color.White);
            SetNavBar();
        }

        private void SetNavBar()
        {

        }

        async void Loaded(System.Object sender, System.EventArgs e)
        {
            string result = await DisplayActionSheet("Nutzername", null, null, new string[]
            {
                "nikeri",
                "tuneke",
                "timbru",
                "nicmis",
                "lasstr"
            });

            AllVM.User = AllVM.Datenbank.User.Get(result);
        }
    }
}