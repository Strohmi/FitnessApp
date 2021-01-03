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
    public partial class TrainingsplanAnsicht : ContentPage
    {
        public Trainingsplan TPlan { get; set; }
        public bool IsFavorite { get; set; }

        public TrainingsplanAnsicht(int id)
        {
            GetByID(id);
            InitializeComponent();
            Start();
            BindingContext = this;
        }

        void Start()
        {
            Title = "Trainingsplan";
            CalculateStars();
        }

        void GetByID(int id)
        {
            TPlan = AllVM.Datenbank.Trainingsplan.GetByID(id);

            if (TPlan != null)
            {
                TPlan.UebungList = AllVM.Datenbank.Trainingsplan.GetUebungen(TPlan.ID);
                IsFavorite = AllVM.Datenbank.User.CheckIfFavo($"T;{TPlan.ID}", AllVM.ConvertToUser());
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Es ist ein Fehler aufgetreten");
        }

        void CalculateStars()
        {
            int count_bew = 5;
            double bewertung = -1;
            if (TPlan.DurchBewertung != -2)
            {
                if (TPlan.DurchBewertung == -1)
                    TPlan.DurchBewertung = 0;

                bewertung = Math.Round((double)TPlan.DurchBewertung * 2, MidpointRounding.AwayFromZero) / 2;
                int count_filled = (int)Math.Floor(bewertung);
                double count_half = bewertung - count_filled;

                if (count_half > 0)
                    count_half = 1;

                for (int i = 0; i < count_bew; i++)
                {
                    Image star = new Image()
                    {
                        Aspect = Aspect.AspectFit,
                        HeightRequest = 25,
                        WidthRequest = 25
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
            //Eigene Bewertungen sollen vermieden werden
            if (TPlan.Ersteller.Nutzername != AllVM.User.Nutzername)
                this.Navigation.PushAsync(new BewertungAdd((sender as Grid).ClassId, typeof(Trainingsplan)));
        }

        void FavoritePlan(System.Object sender, System.EventArgs e)
        {
            Image image = (sender as Image);

            if (image != null && image.ClassId != null)
            {
                string key = "T;" + image.ClassId;
                if ((image.Source as FileImageSource).File == "Herz_Unfilled")
                {
                    if (AllVM.Datenbank.User.AddFavo(key, AllVM.ConvertToUser()))
                        image.Source = ImageSource.FromFile("Herz_Filled");
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Fehler beim Favorisieren");
                }
                else if ((image.Source as FileImageSource).File == "Herz_Filled")
                {
                    if (AllVM.Datenbank.User.DeleteFavo(key, AllVM.ConvertToUser()))
                        image.Source = ImageSource.FromFile("Herz_Unfilled");
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Fehler beim Favorisieren");
                }
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Favorisieren");
        }
    }
}
