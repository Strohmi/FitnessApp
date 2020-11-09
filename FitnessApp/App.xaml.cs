using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.Models.General;
using FitnessApp.Models;
using System.Collections.Generic;

namespace FitnessApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            SetUser();

            TESTER.ListNews = new List<News>()
            {
                new News()
                {
                    Title = "Test Title",
                    Beschreibung = "Hier steht eine Beschreibung drin",
                    Ersteller = new User()
                    {
                        Vorname = "Niklas",
                        Nachname = "Test"
                    },
                    ErstelltAm = new DateTime(2020,09,11)
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
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }

        private void SetUser()
        {
            StaticGlobalVM.User = new User()
            {
                Vorname = "Niklas",
                Nachname = "Erichsen",
                Geburtsdatum = new DateTime(1996, 02, 09)
            };
        }
    }
}
