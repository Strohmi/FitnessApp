using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;
using FitnessApp.ViewModels;

namespace FitnessApp
{
    //Autor: NiE

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

        /// <summary>
        /// Methoden, die erst nach dem Laden ausgeführt werden sollen
        /// </summary>
        void ContentPage_Appearing(System.Object sender, System.EventArgs e)
        {
            FavoPlansVM.ListTrPlan = new List<Trainingsplan>();
            FavoPlansVM.ListErPlan = new List<Ernährungsplan>();
            GetList();
            FavoPlansVM.AnzeigeListe = FavoPlansVM.ListTrPlan.ToList<object>();
        }

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        private void Start()
        {
            Title = "Favoriten";
            NavigationPage.SetIconColor(this, Color.Black);
        }

        /// <summary>
        /// Aktualisierung der Liste
        /// </summary>
        void Refresh(System.Object sender, System.EventArgs e)
        {
            FavoPlansVM.AnzeigeListe = new List<object>();
            GetList();
            (sender as ListView).IsRefreshing = false;
        }

        /// <summary>
        /// Liste aus der Datenbank bereitstellen
        /// </summary>
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

        /// <summary>
        /// Zum Profil springen
        /// </summary>
        void GoToProfil(System.Object sender, System.EventArgs e)
        {
            Image image = (sender as Image);

            if (image.ClassId == AllVM.User.Nutzername)
                this.Navigation.PushAsync(new Profil());
            else
                this.Navigation.PushAsync(new Profil(image.ClassId));
        }

        /// <summary>
        /// Zum Plan springen
        /// </summary>
        void GoToPlan(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var listview = (sender as ListView);
            var item = listview.SelectedItem;
            if (item != null)
            {
                if (item.GetType() == typeof(Trainingsplan))
                    this.Navigation.PushAsync(new TrainingsplanAnsicht((item as Trainingsplan).ID));
                else if (item.GetType() == typeof(Ernährungsplan))
                    this.Navigation.PushAsync(new MahlzeitAnsicht((item as Ernährungsplan).ID));
            }
        }

        /// <summary>
        /// Entscheiden, welche Pläne angezeigt werden sollen
        /// </summary>
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

        /// <summary>
        /// BindingContext der Liste anpassen
        /// </summary>
        void OnBindingContextChanged(System.Object sender, System.EventArgs e)
        {
            MenuItem menuItem = new MenuItem();
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell theViewCell = ((ViewCell)sender);
            var item = theViewCell.BindingContext;
            theViewCell.ContextActions.Clear();

            if (item != null)
            {
                if (item.GetType() == typeof(Trainingsplan))
                {
                    menuItem = new MenuItem()
                    {
                        Text = "Entfernen",
                        ClassId = $"T;{(item as Trainingsplan).ID}",
                        IsDestructive = true
                    };
                    menuItem.Clicked += Delete;
                    theViewCell.ContextActions.Add(menuItem);
                }
                else if (item.GetType() == typeof(Ernährungsplan))
                {
                    menuItem = new MenuItem()
                    {
                        Text = "Entfernen",
                        ClassId = $"E;{(item as Ernährungsplan).ID}",
                        IsDestructive = true
                    };
                    menuItem.Clicked += Delete;
                    theViewCell.ContextActions.Add(menuItem);
                }
            }
        }

        /// <summary>
        /// Beitrag aus den Favoriten löschen
        /// </summary>
        private void Delete(object sender, EventArgs e)
        {
            var menuitem = (sender as MenuItem);

            if (AllVM.Datenbank.User.DeleteFavo(menuitem.ClassId, AllVM.ConvertToUser()))
            {
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich entfernt");
                GetList();
                FavoPlansVM.AnzeigeListe = FavoPlansVM.ListTrPlan.ToList<object>();
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Entfernen");
            }
        }
    }
}