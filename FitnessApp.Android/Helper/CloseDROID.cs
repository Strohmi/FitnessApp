﻿using System;
using Android.App;
using Xamarin.Forms;
using FitnessApp.Models.General;

namespace FitnessApp.Droid.Helper
{
    public class CloseDROID : ICloseApp
    {
        public void Close()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}