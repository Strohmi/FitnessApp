using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.Services;
using FitnessApp.Views;

namespace FitnessApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            //MainPage = new NavigationPage(new AppShell());
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
