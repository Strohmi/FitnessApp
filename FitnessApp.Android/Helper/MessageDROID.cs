﻿using Android.Widget;

namespace FitnessApp.Droid
{
    public class MessageDROID : IMessage
    {
        /// <summary>
        /// Kurze Nachricht anzeigen
        /// </summary>
        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }

        /// <summary>
        /// Lange Nachricht anzeigen
        /// </summary>
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}
