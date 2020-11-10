using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class AddNew : ContentPage
    {
        public AddNewVM AddNewVM { get; set; }

        public AddNew()
        {
            InitializeComponent();
            AddNewVM = new AddNewVM();
            Start();
            BindingContext = AddNewVM;
        }

        private void Loaded(object sender, EventArgs e)
        {
            listOptions.SelectedItem = null;
        }

        private void Start()
        {
            Title = "Hinzufügen";
        }

        async void Selection(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            string msg = null;
            ListView listView = (sender as ListView);

            if (listView != null && listView.SelectedItem != null)
            {
                string result = listView.SelectedItem.ToString();
                msg = result;

                switch (result)
                {
                    case "Status":
                        await this.Navigation.PushAsync(new StatusNew());
                        break;
                    default:
                        await DisplayAlert("Auswahl:", msg, "OK");
                        break;
                }
                listView.SelectedItem = null;
            }
        }
    }
}