using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class StatusNew : ContentPage
    {
        public StatusVM StatusVM { get; set; }
        private bool isFoto;

        public StatusNew()
        {
            InitializeComponent();
            StatusVM = new StatusVM();
            Start();
            BindingContext = StatusVM;
        }

        private void Start()
        {
            Title = "Status hinzufügen";
        }

        void SwitchToggeled(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Switch switcher = (sender as Switch);

            if (switcher.IsToggled)
                isFoto = true;
            else
                isFoto = false;

            if (isFoto)
            {

            }
        }
    }
}