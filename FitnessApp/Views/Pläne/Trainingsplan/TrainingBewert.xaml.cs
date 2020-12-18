using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.Models;
using FitnessApp.Models.General;
using FitnessApp.Models.DB;
using System.Linq;

namespace FitnessApp
{
    public partial class TrainingBewert : ContentPage
    {
        public Trainingsplan TPlan { get; set; }

        public TrainingBewert(int id)
        {
            InitializeComponent();
            Start();
            GetByID(id);
            BindingContext = this;
        }

        void Start()
        {
            Title = "Bewerten!";

            ToolbarItem toolbarItem = new ToolbarItem()
            {
                Text = "Speichern",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };
            toolbarItem.Clicked += Save;
            ToolbarItems.Add(toolbarItem);
        }

        private void Save(object sender, EventArgs e)
        {
            int bewertung = 0;
            foreach (ImageButton item in bewGrid.Children)
            {
                if (item.Source == ImageSource.FromFile("Star_Filled"))
                {
                    bewertung++;
                }
            }

            BewertungTrainingpsplan Tbewert = new BewertungTrainingpsplan()
            {
                Bewerter = AllVM.ConvertToUser(),
                Bewertung = bewertung
            };

            if (AllVM.Datenbank.Trainingsplan.AddBewertung(Tbewert, TPlan))
            {
                OnBackButtonPressed();
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich bewertet");
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Fehler bei Bewertung");
            }
        }

        void GetByID(int id)
        {
            TPlan = AllVM.Datenbank.Trainingsplan.GetByID(id);
        }

        void FillStar(System.Object sender, System.EventArgs e)
        {
            ImageButton button = (sender as ImageButton);

            if (button != null)
            {
                int col = Grid.GetColumn(button);

                if (button.Source == ImageSource.FromFile("Star_Filled"))
                {
                    for (int i = 4; i >= col; i--)
                    {
                        (bewGrid.Children[i] as ImageButton).Source = ImageSource.FromFile("Star_Unfilled");
                    }
                }
                else
                {
                    for (int i = 0; i < col; i++)
                    {
                        (bewGrid.Children[i] as ImageButton).Source = ImageSource.FromFile("Star_Filled");
                    }
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }
    }
}
