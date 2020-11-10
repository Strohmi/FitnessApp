using System;
using Android.Widget;
using FitnessApp.Models.General;

namespace FitnessApp.Droid.Helper
{
    public class MessageDROID : IMessage
    {
        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }

        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}
