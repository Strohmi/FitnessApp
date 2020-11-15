using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class ProfilEdit : ContentPage
    {
        public ProfilVM ProfilVM { get; set; }

        public ProfilEdit()
        {
            InitializeComponent();
            Start();
        }

        private void Loaded(object sender, EventArgs e)
        {
            ProfilVM = new ProfilVM(AllVM.User);
            BindingContext = ProfilVM;
        }

        private void Start()
        {
            Title = "Profil bearbeiten";
            SetNavBar();
        }

        private void SetNavBar()
        {
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

        }
    }
}