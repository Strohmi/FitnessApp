using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

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
            var list = new List<News>()
            {
                new News()
                {
                    Title = "Test Title",
                    Beschreibung = "Hier steht eine Beschreibung drin",
                    Ersteller = new User()
                    {
                        Vorname = "Niklas",
                        Nachname = "Erichsen"
                    },
                    ErstelltAm = DateTime.Now
                },
                new News()
                {
                    Title = "Test Title 2",
                    Beschreibung = "Hier steht eine Beschreibung drin jdausfdismlfim usnduiajsdjiuoasndunasljdnisamd\njduasndinsakdnsaubndiusakldmnasdjas",
                    Ersteller = new User()
                    {
                        Vorname = "Tester",
                        Nachname = "Nachnamemdismfsm"
                    },
                    ErstelltAm = new DateTime(2019,03,20)
                }
            };

            NewsFeedVM.ListNews = list.OrderByDescending(s => s.ErstelltAm).ToList();
        }
    }
}