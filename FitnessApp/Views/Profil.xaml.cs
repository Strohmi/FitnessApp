using System;
using System.ComponentModel;
using FitnessApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp.Views
{
    public partial class Profil : ContentPage
    {
        private ProfilVM profilVM;

        public Profil()
        {
            InitializeComponent();
            BindingContext = profilVM;
        }
    }
}