using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.Models;
using FitnessApp.Models.General;
using FitnessApp.Models.DB;

namespace FitnessApp
{
    public partial class TrainingNew : ContentPage
    {
        public TrainingNew()
        {
            InitializeComponent();
            TrainingVM = new TrainingVM();
            Start();
            BindingContext = TrainingVM;
        }

        public TrainingVM TrainingVM { get; set; }
        public List<Uebung> Uebungen { get; set; }

        private void Start()
        {
            SetNavBar();
            Title = "Trainingsplan erstellen";
            Uebungen = new List<Uebung>();
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

        //Ereigniss Händler zum Hinzufügen einzelner Übungen
        private void Add(object sender, EventArgs e)
        {
            if (CheckEntry())
            {
                WriteEntry();
                ClearEntry();
                trainingview.ItemsSource = null;
                trainingview.ItemsSource = Uebungen;
            }
        }
        private void ClearEntry()
        {
            UebungEntry.Text = null;
            UebungEntry.Placeholder = "Übung";
            WeightEntry.Text = null;
            RepetitionEntry.Text = null;
            SetsEntry.Text = null;
        }
        // Schreibt die Eingaben in die Liste !! Noch keine Sql Übertragung !! 
        private void WriteEntry()
        {

            //SqlID ?? Wird sie hier schon gebraucht oder kann man das auch noch Später machen was wäre sinvoller.
            Uebungen.Add(new Uebung
            {
                Name = UebungEntry.Text,
                Gewicht = Checkdecimal(),
                Repetition = int.Parse(RepetitionEntry.Text),
                Sets = int.Parse(SetsEntry.Text)
            });
        }

        //Wenn der decimal wert nicht angeben wird also kein gewicht automatisch auf null setzen
        private decimal Checkdecimal()
        {
            if (String.IsNullOrEmpty(WeightEntry.Text))
                return 0;
            return decimal.Parse(WeightEntry.Text.Replace(',', '.'));
        }

        // Prüft die Zeilen ob alle Notwendigen angaben vorhanden sind
        private bool CheckEntry()
        {
            if (String.IsNullOrEmpty(UebungEntry.Text) == false && UebungEntry.Text.Equals((string)UebungEntry.Placeholder))
            {
                DependencyService.Get<IMessage>().ShortAlert("Übung eingeben!");
                return false;
            }
            if (int.TryParse(RepetitionEntry.Text, out int result))
            {
                if (int.TryParse(SetsEntry.Text, out int result2))
                    return true;
                DependencyService.Get<IMessage>().ShortAlert("Einheit auswählen!");
                return false;

            }
            DependencyService.Get<IMessage>().ShortAlert("Wiederholungen eingeben!");
            return false;






        }
        // Save Methode funktionsunfähig!!!
        private void Save(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Trainingtitel.Text) && !Trainingtitel.Text.Equals((string)Trainingtitel.Placeholder))
            {
                Trainingsplan tp = new Trainingsplan
                {
                    Titel = Trainingtitel.Text,
                    //User = GetUser oder muss das Übergeben werden?
                    Bewertung = null,
                    ErstelltAm = DateTime.Now,
                    GeAendertAm = DateTime.Now,
                    UebungList = Uebungen
                };
                DB_Trainingsplan dB_Trainingsplan = new DB_Trainingsplan();
                if (dB_Trainingsplan.AddTrainingsplan(tp))
                    DependencyService.Get<IMessage>().ShortAlert("Gespeichert");
                DependencyService.Get<IMessage>().ShortAlert("Ups da hat was nicht geklappt");
            }
            DependencyService.Get<IMessage>().ShortAlert("Geben Sie einen Titel ein");

        }
    }
}
