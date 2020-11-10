using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;

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
            Title = "Feed";
            NavigationPage.SetIconColor(this, Color.White);
            SetNavBar();
        }

        private void SetNavBar()
        {

        }

        void Loaded(System.Object sender, System.EventArgs e)
        {
            NewsFeedVM.ListNews = TESTER.ListNews.OrderByDescending(s => s.ErstelltAm).ToList();
        }
    }
}