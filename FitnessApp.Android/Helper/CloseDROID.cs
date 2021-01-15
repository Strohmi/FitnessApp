using Android.App;
using Xamarin.Forms;

namespace FitnessApp.Droid
{
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
