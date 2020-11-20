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
    public class Meal
    {
        public string Food { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return Amount + Unit + " " + Food;
        }
    }

    public partial class AddMeal : ContentPage
    {
        public string NewFood { get; set; }
        public string NewAmount { get; set; }
        public string NewUnit { get; set; }

        public IList<Meal> Meals { get; private set; }
        //Eventuell mit ObserverableCollection tauschen

        public string[] Units
        {
            get
            {
                return new string[] { "g", "kg", "ml", "l", "Stück" };
            }
        }

        public AddMeal()
        {
            BindingContext = this;
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            Title = "Mahlzeit erstellen";
            SetNavBar();

            Meals = new List<Meal>();

            //Testlebensmittel
            Meals.Add(new Meal { Food = "Kartoffeln", Amount = (decimal)2, Unit = "kg", });
            Meals.Add(new Meal { Food = "Olivenöl", Amount = (decimal)250, Unit = "ml", });
            Meals.Add(new Meal { Food = "Sahne", Amount = (decimal)500, Unit = "ml", });
            Meals.Add(new Meal { Food = "Eier, hart gekocht", Amount = (decimal)8, Unit = "Stück", });
            Meals.Add(new Meal { Food = "Frischkäse", Amount = (decimal)300, Unit = "g", });
            Meals.Add(new Meal { Food = "Oliven", Amount = (decimal)100, Unit = "g", });
            Meals.Add(new Meal { Food = "Bier", Amount = (decimal)1, Unit = "l", });

            mealsView.ItemsSource = null;
            mealsView.ItemsSource = Meals;
            //Testlebensmittel Ende
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

        private void Add(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NewFood))
            {
                if (Convert.ToDecimal(NewAmount) > 0)
                {
                    if (!string.IsNullOrEmpty(NewUnit))
                    {
                        Meals.Add(new Meal
                        {
                            Food = NewFood,
                            Amount = Convert.ToDecimal(NewAmount),
                            Unit = NewUnit
                        });
                        mealsView.ItemsSource = null;
                        mealsView.ItemsSource = Meals;
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

        private void Edit(object sender, EventArgs e)
        {
            DependencyService.Get<IMessage>().ShortAlert("Funktion noch nicht implementiert!");
        }

        private void Save(object sender, EventArgs e)
        {
            DependencyService.Get<IMessage>().ShortAlert("Funktion noch nicht implementiert!");
        }


    }
}