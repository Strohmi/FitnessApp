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
    public partial class BewertungAdd : ContentPage
    {
        public Trainingsplan TPlan { get; set; }
        public Ernährungsplan EPlan { get; set; }
        private Type type;

        public BewertungAdd(string id, Type _type)
        {
            InitializeComponent();
            Start();
            type = _type;
            GetByID(int.Parse(id));
            BindingContext = this;
        }

        void Start()
        {
            Title = "Bewertung";
            ToolbarItem item = new ToolbarItem()
            {
                Text = "Speichern",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };
            item.Clicked += Save;
            ToolbarItems.Add(item);
        }

        private void Save(object sender, EventArgs e)
        {
            int bewert = 0;
            bool correct = false;

            foreach (Image item in bewGrid.Children)
            {
                if ((item.Source as FileImageSource).File == "Star_Filled")
                    bewert++;
            }

            Bewertung bewertung = new Bewertung()
            {
                Bewerter = AllVM.ConvertToUser(),
                Bewertung = bewert
            };

            if (type == typeof(Trainingsplan))
            {
                correct = AllVM.Datenbank.Trainingsplan.AddBewertung(bewertung, TPlan);
            }
            else if (type == typeof(Ernährungsplan))
            {
                correct = AllVM.Datenbank.Ernährungsplan.AddBewertung(bewertung, EPlan);
            }

            if (correct)
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
            if (type == typeof(Trainingsplan))
                TPlan = AllVM.Datenbank.Trainingsplan.GetByID(id);
            else if (type == typeof(Ernährungsplan))
                EPlan = AllVM.Datenbank.Ernährungsplan.GetByID(id);
        }

        private void FillStars(object sender, EventArgs e)
        {
            Image image = (sender as Image);

            if (image != null)
            {
                int col = Grid.GetColumn(image);
                FileImageSource source = image.Source as FileImageSource;

                if (source.File == "Star_Filled")
                {
                    for (int i = 4; i > col; i--)
                    {
                        (bewGrid.Children[i] as Image).Source = ImageSource.FromFile("Star_Unfilled");
                    }
                }
                else if (source.File == "Star_Unfilled")
                {
                    for (int i = 0; i < col + 1; i++)
                    {
                        (bewGrid.Children[i] as Image).Source = ImageSource.FromFile("Star_Filled");
                    }
                }
                else
                {
                    _ = "stop here";
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
