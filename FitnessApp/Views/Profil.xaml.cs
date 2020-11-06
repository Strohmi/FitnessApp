using System;
using System.ComponentModel;
using FitnessApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp.Views
{
    public partial class Profil : ContentPage
    {
        public ProfilVM ProfilVM { get; set; }

        public Profil()
        {
            InitializeComponent();
            ProfilVM = new ProfilVM();
            BindingContext = ProfilVM;
            Start();
        }

        private void Start()
        {
            Title = "Profil";
            SetNavBar();
        }

        private void SetNavBar()
        {
            ToolbarItem item = new ToolbarItem
            {
                Text = "Bearbeiten",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            item.Clicked += Edit;
            ToolbarItems.Add(item);
        }

        private void Edit(object sender, EventArgs e)
        {
            gridEdit.IsVisible = true;

            ToolbarItems.Clear();
            ToolbarItem item = new ToolbarItem
            {
                Text = "Speichern",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            item.Clicked += Save;
            ToolbarItems.Add(item);
        }

        private void Save(object sender, EventArgs e)
        {
            gridEdit.IsVisible = false;
            ToolbarItems.Clear();
            ToolbarItem item = new ToolbarItem
            {
                Text = "Bearbeiten",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            item.Clicked += Edit;
            ToolbarItems.Add(item);

            DisplayAlert("Lüppt", "Gespeichert", "OK");
        }
    }
}