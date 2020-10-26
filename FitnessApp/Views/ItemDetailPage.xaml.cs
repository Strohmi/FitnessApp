using System.ComponentModel;
using Xamarin.Forms;
using FitnessApp.ViewModels;

namespace FitnessApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}