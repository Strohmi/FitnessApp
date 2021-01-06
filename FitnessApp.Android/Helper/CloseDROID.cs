using System;
using Android.App;
using Xamarin.Forms;
using FitnessApp.Models.General;

namespace FitnessApp.Droid.Helper
{
    //Autor: NiE

    public class CloseDROID : ICloseApp
    {
        /// <summary>
        /// App schließen
        /// </summary>
        public void Close()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}
