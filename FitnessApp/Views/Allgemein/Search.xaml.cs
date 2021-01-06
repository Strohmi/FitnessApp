using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.ViewModels;
using FitnessApp.Models;

namespace FitnessApp
{
    //Autor: NiE

    public partial class Search : ContentPage
    {
        public SearchVM SearchVM { get; set; }

        public Search()
        {
            InitializeComponent();
            SearchVM = new SearchVM();
            Start();
            BindingContext = SearchVM;
        }

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        private void Start()
        {
            Title = "Suche";
            SearchVM.Users = AllVM.Datenbank.User.GetList();
            SearchVM.TPläne = AllVM.Datenbank.Trainingsplan.GetList();
            SearchVM.EPläne = AllVM.Datenbank.Ernährungsplan.GetList();
        }

        /// <summary>
        /// Permanente Suche beim Eingeben eines neuen Buchstabens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            Suchen(e.NewTextValue);
        }

        /// <summary>
        /// Suche eines Treffers in der entsprechenden Liste
        /// </summary>
        /// <param name="text"></param>
        void Suchen(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (BTNnutzer.BackgroundColor == Color.LightGreen)
                    SearchVM.UsersResult = SearchVM.Users.Where(s => s.Nutzername.ToLower().Contains(text.ToLower()) | s.CustomName.ToLower().Contains(text.ToLower()) && s.Nutzername != AllVM.User.Nutzername).ToList();
                else if (BTNtrain.BackgroundColor == Color.LightGreen)
                    SearchVM.TPläneResult = SearchVM.TPläne.Where(s => s.Titel.ToLower().Contains(text.ToLower()) || s.Kategorie.ToLower().Contains(text.ToLower()) || s.Ersteller.Nutzername.ToLower().Contains(text.ToLower()) || s.Ersteller.CustomName.ToLower().Contains(text.ToLower())).ToList();
                else if (BTNernä.BackgroundColor == Color.LightGreen)
                    SearchVM.EPläneResult = SearchVM.EPläne.Where(s => s.Titel.ToLower().Contains(text.ToLower()) || s.Kategorie.ToLower().Contains(text.ToLower()) || s.Ersteller.Nutzername.ToLower().Contains(text.ToLower()) || s.Ersteller.CustomName.ToLower().Contains(text.ToLower())).ToList();
            }
            else
            {
                SearchVM.UsersResult = null;
                SearchVM.TPläneResult = null;
                SearchVM.EPläneResult = null;
            }
        }

        /// <summary>
        /// Zu dem entsprechenden Eintrag springen, der als Treffer angezeigt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Tapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ListView list = (sender as ListView);

            if (list == listUser)
                this.Navigation.PushAsync(new Profil((e.Item as User).Nutzername));
            else if (list == listTrain)
                this.Navigation.PushAsync(new TrainingsplanAnsicht((e.Item as Trainingsplan).ID));
            else if (list == listErnäh)
                this.Navigation.PushAsync(new MahlzeitAnsicht((e.Item as Ernährungsplan).ID));
        }

        /// <summary>
        /// Auswahl der Liste, in der gesucht werden soll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonChanged(System.Object sender, EventArgs e)
        {
            Button button = (sender as Button);

            if (button == BTNnutzer)
            {
                listUser.IsVisible = true;
                listTrain.IsVisible = false;
                listErnäh.IsVisible = false;

                BTNnutzer.BackgroundColor = Color.LightGreen;
                BTNtrain.BackgroundColor = Color.White;
                BTNernä.BackgroundColor = Color.White;

                Suchen(searchBar.Text);
            }
            else if (button == BTNtrain)
            {
                listUser.IsVisible = false;
                listTrain.IsVisible = true;
                listErnäh.IsVisible = false;

                BTNnutzer.BackgroundColor = Color.White;
                BTNtrain.BackgroundColor = Color.LightGreen;
                BTNernä.BackgroundColor = Color.White;

                Suchen(searchBar.Text);
            }
            else if (button == BTNernä)
            {
                listUser.IsVisible = false;
                listTrain.IsVisible = false;
                listErnäh.IsVisible = true;

                BTNnutzer.BackgroundColor = Color.White;
                BTNtrain.BackgroundColor = Color.White;
                BTNernä.BackgroundColor = Color.LightGreen;

                Suchen(searchBar.Text);
            }
        }

        /// <summary>
        /// BindingContext eines Items der ListView ändern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnBindingContextChanged(System.Object sender, System.EventArgs e)
        {
            MenuItem menuItem = new MenuItem();
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell theViewCell = ((ViewCell)sender);
            var item = theViewCell.BindingContext;

            if (item != null)
            {
                decimal durchbewert = -1;
                if (item.GetType() == typeof(Trainingsplan))
                    durchbewert = (theViewCell.BindingContext as Trainingsplan).DurchBewertung;
                else if (item.GetType() == typeof(Ernährungsplan))
                    durchbewert = (theViewCell.BindingContext as Ernährungsplan).DurchBewertung;

                int count_bew = 5;
                Grid bewGrid = ((theViewCell.View as Frame).Content as Grid).FindByName<Grid>("bewGrid");
                double bewertung = -1;

                if (durchbewert != -2)
                {
                    if (durchbewert == -1)
                        durchbewert = 0;

                    bewertung = Math.Round((double)durchbewert * 2, MidpointRounding.AwayFromZero) / 2;
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
        }
    }
}