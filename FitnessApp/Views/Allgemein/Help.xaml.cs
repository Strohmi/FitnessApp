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
            string txt = $"Vielen Dank, dass du FitApp benutzt.\n\nHier steht normalerweise der Hilfetext, der aber noch geschrieben werden muss";
            helpText.Text = txt;
        }
    }
}