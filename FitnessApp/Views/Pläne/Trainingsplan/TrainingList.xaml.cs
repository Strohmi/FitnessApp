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
            //SetDataTemplate();
        }

        void Start()
        {
            Title = "Trainingspläne";
        }

        void GetList()
        {
            TPlaene = AllVM.Datenbank.Trainingsplan.GetTrainingsplaene(user.Nutzername);

            if (TPlaene != null)
                TPlaene = TPlaene.OrderByDescending(o => o.ErstelltAm).ToList();
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Laden");

            listView.ItemsSource = null;
            listView.ItemsSource = TPlaene;
        }

        void SetDataTemplate()
        {
            listView.ItemTemplate = new DataTemplate(() =>
            {
                Frame frame = new Frame()
                {
                    Padding = new Thickness(0),
                    Margin = new Thickness(5),
                    HasShadow = false,
                    BorderColor = Color.Black
                };

                var grid = new Grid()
                {
                    RowDefinitions = new RowDefinitionCollection()
                    {
                        new RowDefinition(){Height = GridLength.Star},
                        new RowDefinition(){Height = GridLength.Star}
                    },
                    ColumnDefinitions = new ColumnDefinitionCollection()
                    {
                        new ColumnDefinition(){Width = GridLength.Star},
                        new ColumnDefinition(){Width = GridLength.Star}
                    },
                    Margin = new Thickness(5)
                };

                Label title = new Label()
                {
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.Green
                };
                title.SetBinding(Label.TextProperty, "Titel");
                Grid.SetRow(title, 0);
                Grid.SetColumnSpan(title, 2);
                grid.Children.Add(title);

                Label datum = new Label()
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.End
                };
                datum.SetBinding(Label.TextProperty, "ErstelltAm", BindingMode.Default, null, "{}{0:dd.MM.yyyy}");
                Grid.SetRow(datum, 1);
                Grid.SetColumn(datum, 1);
                grid.Children.Add(datum);

                Grid bewGrid = new Grid()
                {
                    ColumnDefinitions = new ColumnDefinitionCollection()
                    {
                        new ColumnDefinition(){Width = GridLength.Star},
                        new ColumnDefinition(){Width = GridLength.Star},
                        new ColumnDefinition(){Width = GridLength.Star},
                        new ColumnDefinition(){Width = GridLength.Star},
                        new ColumnDefinition(){Width = GridLength.Star}
                    },
                    BackgroundColor = Color.White
                };

                Grid.SetColumn(bewGrid, 0);
                Grid.SetRow(bewGrid, 1);
                grid.Children.Add(bewGrid);

                frame.Content = grid;
                var vc = new ViewCell { View = frame };
                vc.BindingContextChanged += OnBindingContextChanged;
                return vc;
            });
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
                Grid bewGrid = ((theViewCell.View as Frame).Content as Grid).FindByName<Grid>("bewGrid");
                double bewertung = -1;
                if (item.DurchBewertung != -1 && item.DurchBewertung != -2)
                {
                    bewertung = Math.Round((double)item.DurchBewertung * 2, MidpointRounding.AwayFromZero) / 2;
                    int count_filled = (int)Math.Floor(bewertung);

                    for (int i = 0; i < 5; i++)
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
                            star.Source = ImageSource.FromFile("Star_Unfilled");

                        Grid.SetColumn(star, i);
                        bewGrid.Children.Add(star);
                    }
                }

                if (item.User.Nutzername == AllVM.User.Nutzername)
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
