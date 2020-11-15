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

        void Loaded(System.Object sender, System.EventArgs e)
        {

        }

        void GoToSearch(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new ProfilSearch());
        }

        void GoToChats(System.Object sender, System.EventArgs e)
        {
        }
    }
}