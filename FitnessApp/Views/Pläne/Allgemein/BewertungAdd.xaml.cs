using System;
using Xamarin.Forms;

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

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
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

        /// <summary>
        /// Die aktuelle Bewertung speichern
        /// </summary>
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
                Rating = bewert
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

        /// <summary>
        /// Herausfinden, welcher Plan bewertet werden soll
        /// </summary>
        void GetByID(int id)
        {
            if (type == typeof(Trainingsplan))
                TPlan = AllVM.Datenbank.Trainingsplan.GetByID(id);
            else if (type == typeof(Ernährungsplan))
                EPlan = AllVM.Datenbank.Ernährungsplan.GetByID(id);
        }

        /// <summary>
        /// Visuelle Darstellung der Bewertung
        /// </summary>
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
            }
        }

        /// <summary>
        /// Seite aus dem Stack löschen
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }
    }
}
