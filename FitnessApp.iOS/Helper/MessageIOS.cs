using Foundation;
using UIKit;

namespace FitnessApp.iOS
{
    public class MessageIOS : IMessage
    {
        const double SHORT_DELAY = 1.5;
        const double LONG_DELAY = 3.0;

        NSTimer alertDelay;
        UIAlertController alert;

        /// <summary>
        /// Kurze Nachricht anzeigen
        /// </summary>
        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        /// <summary>
        /// Lange Nachricht anzeigen
        /// </summary>
        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }

        /// <summary>
        /// Nachricht anzeigen
        /// </summary>
        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        /// <summary>
        /// Nachricht nicht mehr anzeigen
        /// </summary>
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
