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
    //Autor: NiE

    public partial class TrainingList : ContentPage
    {
        public List<Trainingsplan> TPlaene { get; set; }
        private User user;

        public TrainingList(User _user)
        {
            InitializeComponent();
            user = _user;
            Start();
            BindingContext = this;
        }

        void Loaded(System.Object sender, System.EventArgs e)
        {
            GetList();
        }

        void Start()
        {
            Title = "Trainingspläne";
        }

        void GetList()
        {
            TPlaene = AllVM.Datenbank.Trainingsplan.GetList(user.Nutzername);

            if (TPlaene != null)
                TPlaene = TPlaene.OrderByDescending(o => o.ErstelltAm).ToList();
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Laden");

            listView.ItemsSource = null;
            listView.ItemsSource = TPlaene;
        }

        void GoToPlan(System.Object sender, ItemTappedEventArgs e)
        {
            var item = (e.Item as Trainingsplan);

            if (item != null)
                this.Navigation.PushAsync(new TrainingsplanAnsicht(item.ID));
        }

        void OnBindingContextChanged(System.Object sender, System.EventArgs e)
        {
            MenuItem menuItem = new MenuItem();
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell theViewCell = ((ViewCell)sender);
            Trainingsplan item = theViewCell.BindingContext as Trainingsplan;
            theViewCell.ContextActions.Clear();

            if (item != null)
            {
                int count_bew = 5;
                Grid bewGrid = ((theViewCell.View as Frame).Content as Grid).FindByName<Grid>("bewGrid");
                double bewertung = -1;
                if (item.DurchBewertung != -2)
                {
                    if (item.DurchBewertung == -1)
                        item.DurchBewertung = 0;

                    bewertung = Math.Round((double)item.DurchBewertung * 2, MidpointRounding.AwayFromZero) / 2;
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

                if (item.Ersteller.Nutzername == AllVM.User.Nutzername)
                {
                    menuItem = new MenuItem()
                    {
                        Text = "Löschen",
                        ClassId = $"{item.ID}",
                        IsDestructive = true
                    };
                    menuItem.Clicked += Delete;
                    theViewCell.ContextActions.Add(menuItem);
                }
            }
        }

        private void Delete(object sender, EventArgs e)
        {
            Trainingsplan plan = TPlaene.Find(s => s.ID.ToString() == (sender as MenuItem).ClassId);
            if (AllVM.Datenbank.Trainingsplan.Delete(plan))
            {
                GetList();
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gelöscht");
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Löschen");
        }
    }
}
