using System;
using System.Collections.Generic;
using Xamarin.Forms;
using FitnessApp.Models;
using FitnessApp.Models.General;

namespace FitnessApp
{
    public partial class TrainingNew : ContentPage
    {
        //Bindings im XAML-Code übergeben an diese Eigenschaften
        public string TrainingName { get; set; }
        public string TrainingCategorie { get; set; }

        public string NewName { get; set; }
        public string NewSätze { get; set; }
        public string NewWiederholungen { get; set; }
        public string NewMenge { get; set; }
        public string NewEinheit { get; set; }

        //Eigenschaft wird erstellt, um die Einheiten aus der Datenbank abzurufen und im Picker beim hinzufügen von Trainings darzustellen
        public List<string> Units { get; set; }

        public List<string> Categories { get; set; }

        //Eine neue Instanz "plan" von Trainingsplan wird erstellt
        Trainingsplan plan = new Trainingsplan();

        public TrainingNew()
        {
            InitializeComponent();
            Start();
            BindingContext = this;
        }

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        public void Start()
        {
            SetNavBar();
            Title = "Training erstellen";

            //Diese Methoden müssen noch geschrieben werden!
            Units = AllVM.Datenbank.Trainingsplan.GetUnits();
            Categories = AllVM.Datenbank.Trainingsplan.GetCategories();
        }

        /// <summary>
        /// Nagivationsbar einstellen
        /// </summary>
        private void SetNavBar()
        {
            ToolbarItem item = new ToolbarItem
            {
                Text = "Speichern",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            item.Clicked += Save;
            ToolbarItems.Add(item);
        }

        /// <summary>
        /// Eine Übung zum Trainingsplan hinzufügen
        /// </summary>
        private void Add(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NewName))
                {
                    if (Convert.ToDecimal(NewMenge) > 0)
                    {
                        if (!string.IsNullOrEmpty(NewEinheit))
                        {
                            plan.UebungList.Add(new Uebung
                            {
                                Name = NewName,
                                Sätze = Convert.ToInt32(NewSätze),
                                Wiederholungen = Convert.ToInt32(NewWiederholungen),
                                Menge = Convert.ToDecimal(NewMenge),
                                Einheit = NewEinheit
                            });
                            //Hinweis: Wenn Sätze und/oder Wiederholungen null sind, wird 0 übergeben

                            ClearRefresh();
                        }
                        else
                            DependencyService.Get<IMessage>().ShortAlert("Einheit auswählen!");
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Menge auswählen!");
                }
                else
                    DependencyService.Get<IMessage>().ShortAlert("Namen eingeben!");
            }
            catch
            {
                //Anzeige einer Meldung eine fehlgeschlagene Speicherung
                DependencyService.Get<IMessage>().ShortAlert("Ein unbekannter Fehler ist aufgetreten!");
            }
        }

        /// <summary>
        /// Übung für die nächste Eingabe bereit machen
        /// </summary>
        private void ClearRefresh()
        {
            nameEntry.Text = null;
            sätzeEntry.Text = null;
            wiederholungenEntry.Text = null;
            mengeEntry.Text = null;

            listView.ItemsSource = null;
            listView.ItemsSource = plan.UebungList;
        }

        /// <summary>
        /// Plan in die Datenbank speichern
        /// </summary>
        private void Save(object sender, EventArgs e)
        {
            //Übergabe von User, Mahlzeittitel und aktuellen Datum an Instanz "plan" von der Klasse Trainingsplan
            try
            {
                if (!string.IsNullOrEmpty(TrainingName))
                {
                    if (!string.IsNullOrEmpty(TrainingCategorie))
                    {
                        plan.Ersteller = AllVM.ConvertToUser();
                        plan.ErstelltAm = DateTime.Now;
                        plan.Titel = TrainingName.Replace("\n", "");
                        plan.Kategorie = TrainingCategorie;

                        //Die Instanz "plan" von Trainingsplan wird zur Speicherung in der Datenbank an die Methode "AddTrainingsplan" übergeben
                        if (AllVM.Datenbank.Trainingsplan.AddTrainingsplan(plan))
                        {
                            //Rückkehr zur Ansicht "AddNew"
                            OnBackButtonPressed();
                            //Anzeige einer Meldung für die erfolgreiche Speicherung
                            DependencyService.Get<IMessage>().ShortAlert("Training wurde gespeichert!");
                        }
                        else
                            //Anzeige einer Meldung für die fehlerhafte Speicherung
                            DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern");
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Kategorie auswählen!");
                }
                else
                    DependencyService.Get<IMessage>().ShortAlert("Namen eingeben!");
            }
            catch
            {
                //Anzeige einer Meldung eine fehlgeschlagene Speicherung
                DependencyService.Get<IMessage>().ShortAlert("Ein unbekannter Fehler ist aufgetreten!");
            }
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
