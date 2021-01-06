using System;
using Android.App;
using Xamarin.Forms;
using FitnessApp.Models.General;

namespace FitnessApp.Droid.Helper
{
    //Autor: Niklas Erichsen

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
