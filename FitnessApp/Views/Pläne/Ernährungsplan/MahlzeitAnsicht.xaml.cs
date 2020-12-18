using System.Collections.Generic;
using Xamarin.Forms;
using FitnessApp.Models;
using FitnessApp.Models.General;
using System;

namespace FitnessApp
{
    public partial class MahlzeitAnsicht : ContentPage
    {
        public List<Mahlzeiten> MahlzeitenList { get; set; }
        private Ernährungsplan EPlan;
        public bool IsFavorite { get; set; }

        public MahlzeitAnsicht(int id)
        {
            InitializeComponent();
            Start();
            GetByID(id);
            BindingContext = this;
        }

        void Start()
        {
            Title = "Ernährungsplan";
        }

        void GetByID(int id)
        {
            EPlan = AllVM.Datenbank.Ernährungsplan.GetByID(id);
            IsFavorite = AllVM.Datenbank.User.CheckIfFavo($"E;{EPlan.ID}", AllVM.ConvertToUser());
            CalculateStars();
        }

        void CalculateStars()
        {
            int count_bew = 5;
            double bewertung = -1;
            if (EPlan.DurchBewertung != -1 && EPlan.DurchBewertung != -2)
            {
                bewertung = Math.Round((double)EPlan.DurchBewertung * 2, MidpointRounding.AwayFromZero) / 2;
                int count_filled = (int)Math.Floor(bewertung);
                double count_half = bewertung - count_filled;

                if (count_half > 0)
                    count_half = 1;

                for (int i = 0; i < count_bew; i++)
                {
                    Image star = new Image()
                    {
                        Aspect = Aspect.AspectFit,
                        HeightRequest = 20,
                        WidthRequest = 20
                    };

                    if (i <= count_filled - 1)
                    {
                        star.Source = ImageSource.FromFile("Star_Filled");
                    }
                    else
                    {
                        if (count_half == 1)
                        {
                            star.Source = ImageSource.FromFile("Star_HalfFilled");
                            count_half = 0;
                        }
                        else
                            star.Source = ImageSource.FromFile("Star_Unfilled");
                    }

                    Grid.SetColumn(star, i);
                    bewGrid.Children.Add(star);
                }
            }
        }

        void GoToProfil(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new Profil((sender as Label).ClassId));
        }

        void GoToBewertung(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new BewertungAdd((sender as Grid).ClassId, typeof(Ernährungsplan)));
        }

        void FavoritePlan(System.Object sender, System.EventArgs e)
        {
            Image image = (sender as Image);

            if (image != null)
            {
                string key = "E;" + image.ClassId;
                if ((image.Source as FileImageSource).File == "Star_Unfilled")
                {
                    if (AllVM.Datenbank.User.AddFavo(key, AllVM.ConvertToUser()))
                        image.Source = ImageSource.FromFile("Star_Filled");
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Fehler beim Favorisieren");
                }
                else if ((image.Source as FileImageSource).File == "Star_Filled")
                {
                    if (AllVM.Datenbank.User.DeleteFavo(key, AllVM.ConvertToUser()))
                        image.Source = ImageSource.FromFile("Star_Unfilled");
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Fehler beim Favorisieren");
                }
            }
        }
    }
}
