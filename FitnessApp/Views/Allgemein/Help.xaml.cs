using Xamarin.Forms;

namespace FitnessApp
{
    public partial class Help : ContentPage
    {
        public Help()
        {
            InitializeComponent();
            SetHelpText();
        }

        /// <summary>
        /// Hilfetext einfügen
        /// </summary>
        private void SetHelpText()
        {
            string txt = $"Vielen Dank, dass du FitApp benutzt.\n\nHier könnte in Zukunft ein Hilfetext stehen.";
            helpText.Text = txt;
        }
    }
}