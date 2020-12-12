using System;
using Foundation;
using UIKit;
using FitnessApp.Models.General;

namespace FitnessApp.iOS.Helper
{
    //Autor: NiE

    public class MessageIOS : IMessage
    {
        const double SHORT_DELAY = 1.5;
        const double LONG_DELAY = 3.0;

        NSTimer alertDelay;
        UIAlertController alert;

        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }

        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }

            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}
