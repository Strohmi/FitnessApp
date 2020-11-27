﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using FitnessApp.Models.General;
using FitnessApp.Views.Chat;
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
                KeyValuePair<int, string> result = (KeyValuePair<int, string>)listView.SelectedItem;
                msg = result.ToString();

                switch (result.Value)
                {
                    case "Status":
                        await this.Navigation.PushAsync(new StatusNew());
                        break;
                    case "Trainingsplan":
                        await this.Navigation.PushAsync(new TrainingNew());
                        break;
                    case "Ernährungsplan":
                        await this.Navigation.PushAsync(new AddMeal());
                        break;
                    case "Chat":
                        await this.Navigation.PushAsync(new ChatView());
                        break;
                    case "Message":
                        await this.Navigation.PushAsync(new MessageView());
                        break;
                    default:
                        await DisplayAlert("Auswahl:", msg, "OK");
                        break;
                }
                listView.SelectedItem = null;
            }
        }

        private void Test()
        {

        }
    }
}