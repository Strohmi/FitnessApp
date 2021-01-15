using Xamarin.Forms;

namespace FitnessApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Profil), typeof(Profil));
            Routing.RegisterRoute(nameof(FitFeed), typeof(FitFeed));
        }
    }
}
