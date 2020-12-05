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
    public partial class MahlzeitList : ContentPage
    {
        public List<Ernährungsplan> EPlaene { get; set; }
        private User user;

        public MahlzeitList(User _user)
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
            Title = "Ernährungspläne";
        }

        void GetList()
        {
            EPlaene = AllVM.Datenbank.Ernährungsplan.GetErnährungsplaene(user.Nutzername);

            if (EPlaene != null)
                EPlaene = EPlaene.OrderByDescending(o => o.ErstelltAm).ToList();
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Laden");
        }

        void OnBindingContextChanged(System.Object sender, System.EventArgs e)
        {
            MenuItem menuItem = new MenuItem();
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell theViewCell = ((ViewCell)sender);
            Ernährungsplan item = theViewCell.BindingContext as Ernährungsplan;
            theViewCell.ContextActions.Clear();

            if (item != null)
            {
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
            Ernährungsplan plan = EPlaene.Find(s => s.ID.ToString() == (sender as MenuItem).ClassId);
            if (AllVM.Datenbank.Ernährungsplan.Delete(plan))
            {
                GetList();
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gelöscht");
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Löschen");
        }
    }
}
