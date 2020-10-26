using System;
using System.Collections.Generic;
using FitnessApp.ViewModels;
using FitnessApp.Views;
using Xamarin.Forms;

namespace FitnessApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(Profil), typeof(Profil));
        }

    }
}
