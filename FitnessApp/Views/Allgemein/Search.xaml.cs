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

        private void Start()
        {
            Title = "Suche";
            SearchVM.Users = AllVM.Datenbank.User.GetList();
            SearchVM.TPläne = AllVM.Datenbank.Trainingsplan.GetList();
            SearchVM.EPläne = AllVM.Datenbank.Ernährungsplan.GetList();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            Suchen(e.NewTextValue);
        }

        void Suchen(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (RBnutzer.IsChecked == true)
                    SearchVM.UsersResult = SearchVM.Users.Where(s => s.Nutzername.ToLower().Contains(text.ToLower()) && s.Nutzername != AllVM.User.Nutzername).ToList();
                else if (RBtrain.IsChecked == true)
                    SearchVM.TPläneResult = SearchVM.TPläne.Where(s => s.Titel.ToLower().Contains(text.ToLower()) || s.Kategorie.ToLower().Contains(text.ToLower()) || s.User.Nutzername.ToLower().Contains(text.ToLower())).ToList();
                else if (RBernä.IsChecked == true)
                    SearchVM.EPläneResult = SearchVM.EPläne.Where(s => s.Titel.ToLower().Contains(text.ToLower()) || s.Kategorie.ToLower().Contains(text.ToLower()) || s.User.Nutzername.ToLower().Contains(text.ToLower())).ToList();
            }
            else
            {
                SearchVM.UsersResult = null;
                SearchVM.TPläneResult = null;
                SearchVM.EPläneResult = null;
            }
        }

        void Tapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ListView list = (sender as ListView);

            if (list == listUser)
            {
                this.Navigation.PushAsync(new Profil((e.Item as User).Nutzername));
            }
            else if (list == listTrain)
            {
                DependencyService.Get<IMessage>().ShortAlert("Hier sehen Sie einen Trainingsplan");
            }
            else if (list == listErnäh)
            {
                DependencyService.Get<IMessage>().ShortAlert("Hier sehen Sie einen Ernärhungsplan");
            }
        }

        void CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            RadioButton radio = (sender as RadioButton);

            if (radio == RBnutzer)
            {
                listUser.IsVisible = true;
                listTrain.IsVisible = false;
                listErnäh.IsVisible = false;
                Suchen(searchBar.Text);
            }
            else if (radio == RBtrain)
            {
                listUser.IsVisible = false;
                listTrain.IsVisible = true;
                listErnäh.IsVisible = false;
                Suchen(searchBar.Text);
            }
            else if (radio == RBernä)
            {
                listUser.IsVisible = false;
                listTrain.IsVisible = false;
                listErnäh.IsVisible = true;
                Suchen(searchBar.Text);
            }
        }

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

                if (durchbewert != -1 && durchbewert != -2)
                {
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