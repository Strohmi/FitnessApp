using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;
using System.Threading.Tasks;
using System.Timers;
using FitnessApp.ViewModels;

namespace FitnessApp
{
    public partial class FavoPlans : ContentPage
    {
        public FavoPlansVM FavoPlansVM { get; set; }

        public FavoPlans()
        {
            InitializeComponent();
            FavoPlansVM = new FavoPlansVM();
            Start();
            BindingContext = FavoPlansVM;
        }

        private void Start()
        {
            Title = "Favoriten";
            NavigationPage.SetIconColor(this, Color.Black);

            FavoPlansVM.ListTrPlan = new List<Trainingsplan>();
            FavoPlansVM.ListErPlan = new List<Ernährungsplan>();
            GetList();
            FavoPlansVM.AnzeigeListe = FavoPlansVM.ListTrPlan.ToList<object>();
        }

        void Loaded(System.Object sender, System.EventArgs e)
        {

        }

        void Refresh(System.Object sender, System.EventArgs e)
        {
            FavoPlansVM.AnzeigeListe = new List<object>();
            GetList();
            (sender as ListView).IsRefreshing = false;
        }

        private void GetList()
        {
            FavoPlansVM.ListFavoPlans = AllVM.Datenbank.User.GetFavoPlans();

            foreach (var item in FavoPlansVM.ListFavoPlans)
            {
                switch (item.Typ)
                {
                    case "T":
                        FavoPlansVM.ListTrPlan.Add(AllVM.Datenbank.Trainingsplan.GetByID(item.ID));
                        break;
                    case "E":
                        FavoPlansVM.ListErPlan.Add(AllVM.Datenbank.Ernährungsplan.GetByID(item.ID));
                        break;
                    default:
                        break;
                }
            }

            if (FavoPlansVM.ListErPlan == null || FavoPlansVM.ListTrPlan == null)
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Abruf der Liste");
        }

        void GoToProfil(System.Object sender, System.EventArgs e)
        {
            Image image = (sender as Image);

            if (image.ClassId == AllVM.User.Nutzername)
                this.Navigation.PushAsync(new Profil());
            else
                this.Navigation.PushAsync(new Profil(image.ClassId));
        }

        void ChangeCategory(object sender, EventArgs e)
        {
            Button button = (sender as Button);

            if (button == btnTrain)
            {
                btnTrain.BackgroundColor = Color.LightGreen;
                btnErnae.BackgroundColor = Color.White;
                FavoPlansVM.AnzeigeListe = FavoPlansVM.ListTrPlan.ToList<object>();
            }
            else if (button == btnErnae)
            {
                btnErnae.BackgroundColor = Color.LightGreen;
                btnTrain.BackgroundColor = Color.White;
                FavoPlansVM.AnzeigeListe = FavoPlansVM.ListErPlan.ToList<object>();
            }
        }

        void OnBindingContextChanged(System.Object sender, System.EventArgs e)
        {

        }
    }
}