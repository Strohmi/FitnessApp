using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;
using System.Threading.Tasks;

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
            NavigationPage.SetIconColor(this, Color.White);
        }

        void Loaded(System.Object sender, System.EventArgs e)
        {
            GetList();
        }

        void GoToSearch(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new ProfilSearch());
        }

        void GoToChats(System.Object sender, System.EventArgs e)
        {
        }

        void Refresh(System.Object sender, System.EventArgs e)
        {
            GetList();
            (sender as ListView).IsRefreshing = false;
        }

        private void GetList()
        {
            NewsFeedVM.ListNews = AllVM.Datenbank.Feed.Get();
            if (NewsFeedVM.ListNews != null)
                NewsFeedVM.ListNews = NewsFeedVM.ListNews.OrderByDescending(o => o.ErstelltAm).ToList();
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Abruf der Liste");
        }
    }
}