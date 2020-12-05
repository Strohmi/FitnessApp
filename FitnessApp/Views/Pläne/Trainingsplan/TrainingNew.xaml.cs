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

        //Die Eigenschaft wird erstellt, um die Einheiten aus der Datenbank abzurufen und im Picker beim hinzufügen von Trainings darzustellen
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

        public void Start()
        {
            SetNavBar();
            Title = "Training erstellen";

            //Diese Methoden müssen noch geschrieben werden!
            //Units = AllVM.Datenbank.Trainingsgsplan.GetUnits();
            //Categories = AllVM.Datenbank.Trainingsplan.GetCategories();
        }

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

        //Diese Methode fügt einen neuen Eintrag mit Namen, Menge und Einheit zur Liste "plan.TrainingList" hinzu
        private void Add(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        //Diese Methode speichert alle Eingaben in der Datenbank
        private void Save(object sender, EventArgs e)
        {
            //Übergabe von User, Mahlzeittitel und aktuellen Datum an Instanz "plan" von der Klasse Trainingsplan
            plan.User = AllVM.ConvertToUser();
            plan.ErstelltAm = DateTime.Now;
            plan.Titel = TrainingName;
            plan.Kategorie = TrainingCategorie;

            try
            {
                if (!string.IsNullOrEmpty(TrainingName))
                {
                    if (!string.IsNullOrEmpty(TrainingCategorie))
                    {
                        //Die Instanz "plan" von Trainingsplan wird zur Speicherung in der Datenbank an die Methode "AddTrainingsplan" übergeben
                        AllVM.Datenbank.Trainingsplan.AddTrainingsplan(plan);

                        //Anzeige einer Meldung für die erfolgreiche Speicherung
                        DependencyService.Get<IMessage>().ShortAlert("Training wurde gespeichert!");

                        //Rückkehr zur Ansicht "AddNew"
                        OnBackButtonPressed();
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
    }
}
