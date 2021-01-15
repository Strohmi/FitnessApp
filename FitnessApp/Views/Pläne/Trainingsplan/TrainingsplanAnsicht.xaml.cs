using System;
using Xamarin.Forms;

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

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        void Start()
        {
            Title = "Trainingsplan";
            CalculateStars();
        }

        /// <summary>
        /// Plan mit Hilfe der ID aus der Datenbank
        /// </summary>
        /// <param name="id">ID des Plans</param>
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

        /// <summary>
        /// Berechnung und Anzeige der Bewertung des Plans
        /// </summary>
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

        /// <summary>
        /// Zum Profil springen
        /// </summary>
        void GoToProfil(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new Profil((sender as Label).ClassId));
        }

        /// <summary>
        /// Bewertung hinzufügen
        /// </summary>
        void GoToBewertung(System.Object sender, System.EventArgs e)
        {
            //Eigene Bewertungen sollen vermieden werden
            if (TPlan.Ersteller.Nutzername != AllVM.User.Nutzername)
                this.Navigation.PushAsync(new BewertungAdd((sender as Grid).ClassId, typeof(Trainingsplan)));
        }

        /// <summary>
        /// Plan als Favorit kennzeichnen
        /// </summary>
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

        /// <summary>
        /// Seite aus Stack löschen
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }
    }
}
