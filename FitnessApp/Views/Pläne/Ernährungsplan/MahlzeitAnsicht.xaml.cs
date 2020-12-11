﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.Models;
using FitnessApp.Models.General;
using FitnessApp.Models.DB;
using System.Linq;

namespace FitnessApp
{
    public partial class MahlzeitAnsicht : ContentPage
    {
        public List<Mahlzeiten> MahlzeitenList { get; set; }
        private Ernährungsplan Ernährungsplan;

        public MahlzeitAnsicht(Ernährungsplan _ernährungsplan)
        {
            InitializeComponent();
            Ernährungsplan = _ernährungsplan;
            Start();
            BindingContext = this;
        }

        void Loaded(System.Object sender, System.EventArgs e)
        {
            GetList();
        }

        void Start()
        {
            Title = "Ernährungsplan";
        }

        void GetList()
        {
          // Not Implementet
        }

       

       
    }
}
