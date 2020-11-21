﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class Follower : ContentPage
    {
        public User user { get; set; }
        public List<Models.Follower> Follows { get; set; }

        public Follower(User _user)
        {
            InitializeComponent();
            user = _user;
            Start();
            BindingContext = this;
        }

        private void Start()
        {
            Title = "Follower";
            var users = AllVM.Datenbank.User.GetFollows(user.Nutzername);
            if (users != null)
            {
                Follows = users.OrderBy(s => s.User.Nutzername).ThenBy(o => o.GefolgtAm).ToList();
            }
            else
            {
                OnBackButtonPressed();
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Laden");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }
    }
}