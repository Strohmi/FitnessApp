using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.Models.General;
using FitnessApp.Models;

namespace FitnessApp
{
    public partial class StatusNew : ContentPage
    {
        public StatusVM StatusVM { get; set; }
        private bool isFoto;
        private bool isRichtig;

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
            NavigationPage.SetBackButtonTitle(this, "Auswahl");

            ToolbarItem item = new ToolbarItem()
            {
                Text = "Speichern",
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };
            item.Clicked += Save;
            ToolbarItems.Add(item);
        }

        private void Save(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(StatusVM.Status.Title))
            {
                if (isFoto)
                {
                    isRichtig = false;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(StatusVM.Status.Beschreibung))
                    {
                        isRichtig = true;
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Beschreibung füllen!");
                    }
                }

                if (isRichtig)
                {
                    TESTER.ListNews.Add(new News()
                    {
                        Title = StatusVM.Status.Title,
                        Beschreibung = StatusVM.Status.Beschreibung,
                        Ersteller = StaticGlobalVM.User,
                        ErstelltAm = DateTime.Now
                    });

                    OnBackButtonPressed();
                    DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gespeichert");
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Titel füllen!");
            }
        }

        void SwitchToggeled(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Switch switcher = (sender as Switch);

            if (switcher.IsToggled)
                isFoto = true;
            else
                isFoto = false;

            Change();
        }

        private void Change()
        {
            btFoto.IsVisible = isFoto;
            beschreibung.IsVisible = !isFoto;
        }

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }
    }
}