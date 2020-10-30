using System;
using System.ComponentModel;
using FitnessApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp.Views
{
    public partial class Profil : ContentPage
    {
        public DateTime MinDate { get { return new DateTime(1900, 01, 01); } }
        public DateTime MaxDate { get { return DateTime.Now.Date.AddYears(-10); } } //Man muss mindestens 10 Jahre alt sein
        public ProfilVM ProfilVM { get; set; }

        public Profil()
        {
            InitializeComponent();
            ProfilVM = new ProfilVM();
            BindingContext = this;
        }

        private void Start()
        {
            Title = "Profil";
            SetNavBar();
        }

        private void SetNavBar()
        {
            //NavigationPage.SetHasBackButton(this, true);
            //NavigationPage.SetBackButtonTitle(this, "");

            //ToolbarItem item = new ToolbarItem
            //{
            //    Text = "Bearbeiten",
            //    Order = ToolbarItemOrder.Primary,
            //    Priority = 0
            //};
            //item.Clicked += Edit;
            //ToolbarItems.Add(item);
        }

        private void Edit(object sender, EventArgs e)
        {
            gridEdit.IsVisible = true;

            //ToolbarItems.Clear();
            //ToolbarItem item = new ToolbarItem
            //{
            //    Text = "Speichern",
            //    Order = ToolbarItemOrder.Primary,
            //    Priority = 0
            //};
            //item.Clicked += Save;
            //ToolbarItems.Add(item);
        }

        private void Save(object sender, EventArgs e)
        {
            DisplayAlert("Lüppt", "Gespeichert", "OK");
        }
    }
}