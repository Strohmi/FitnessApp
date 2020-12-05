using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using FitnessApp.Models;
using FitnessApp.Models.General;

namespace FitnessApp
{
    public partial class MahlzeitNew : ContentPage
    {
        //Bindings im XAML-Code übergeben an diese Eigenschaften
        public string MealName { get; set; }
        public string MealCategorie { get; set; }

        public string NewFood { get; set; }
        public string NewAmount { get; set; }
        public string NewUnit { get; set; }

        //Eine neue Instanz "plan" von Ernährungsplan wird erstellt
        Ernährungsplan plan = new Ernährungsplan();

        //Die Eigenschaft wird erstellt, um die Einheiten aus der Datenbank abzurufen und im Picker beim hinufügen von Nahrungen darzustellen
        public List<string> Units { get; set; }

        public List<string> Cateogries { get; set; }

        public MahlzeitNew()
        {
            InitializeComponent();
            Start();
            BindingContext = this;
        }

        public void Start()
        {
            Title = "Mahlzeit erstellen";
            SetNavBar();

            //Diese Methoden müssen noch geschrieben werden!
            //Units = AllVM.Datenbank.Ernährungsplan.GetUnits();
            //Cateogries = AllVM.Datenbank.Ernährungsplan.GetCategories();
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

        //Diese Methode fügt einen neuen Eintrag mit Namen, Menge und Einheit zur Liste "plan.MahlzeitenList" hinzu
        private void Add(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NewFood))
                {
                    if (Convert.ToDecimal(NewAmount) > 0)
                    {
                        if (!string.IsNullOrEmpty(NewUnit))
                        {
                            plan.MahlzeitenList.Add(new Mahlzeiten
                            {
                                Nahrungsmittel = NewFood,
                                Menge = Convert.ToDecimal(NewAmount),
                                Einheit = NewUnit
                            });

                            foodEntry.Text = null;
                            amountEntry.Text = null;

                            mealsView.ItemsSource = null;
                            mealsView.ItemsSource = plan.MahlzeitenList;
                        }
                        else
                            DependencyService.Get<IMessage>().ShortAlert("Einheit auswählen!");
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Menge eingeben!");
                }
                else
                    DependencyService.Get<IMessage>().ShortAlert("Lebensmittel benennen!");
            }
            catch
            {
                //Anzeige einer Meldung eine fehlgeschlagene Speicherung
                DependencyService.Get<IMessage>().ShortAlert("Ein unbekannter Fehler ist aufgetreten!");
            }
        }

        //Diese Methode speichert alle Eingaben in der Datenbank
        private void Save(object sender, EventArgs e)
        {
            //Übergabe von User, Mahlzeittitel und aktuellen Datum an Instanz "plan" von der Klasse Ernährungsplan
            plan.User = AllVM.ConvertToUser();
            plan.ErstelltAm = DateTime.Now;
            plan.Titel = MealName;
            plan.Kategorie = MealCategorie;

            try
            {
                if (!string.IsNullOrEmpty(MealName))
                {
                    if (!string.IsNullOrEmpty(MealCategorie))
                    {
                        //Die Instanz "plan" von Ernährungsplan wird zur Speicherung in der Datenbank an die Methode "AddErnährungsplan" übergeben
                        AllVM.Datenbank.Ernährungsplan.AddErnährungsplan(plan);

                        //Anzeige einer Meldung für die erfolgreiche Speicherung
                        DependencyService.Get<IMessage>().ShortAlert("Mahlzeit wurde gespeichert!");

                        //Rückkehr zur Ansicht "AddNew"
                        OnBackButtonPressed();
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Kateogrie auswählen!");
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