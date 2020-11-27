using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class ProfilSearch : ContentPage
    {
        public ProfilSearchVM ProfilVM { get; set; }

        public ProfilSearch()
        {
            InitializeComponent();
            ProfilVM = new ProfilSearchVM();
            Start();
            BindingContext = ProfilVM;
        }

        private void Start()
        {
            Title = "Profilsuche";
            ProfilVM.Users = AllVM.Datenbank.User.GetList();
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            SearchBar search = (sender as SearchBar);

            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                ProfilVM.UsersResult = ProfilVM.Users.Where(s => s.Nutzername.ToLower().Contains(e.NewTextValue.ToLower()) && s.Nutzername != AllVM.User.Nutzername).ToList();
            }
            else
            {
                ProfilVM.UsersResult = null;
            }
        }

        void UserTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            this.Navigation.PushAsync(new Profil((e.Item as User).Nutzername));
        }
    }
}