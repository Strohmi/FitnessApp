using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using FitnessApp.Models;
using FitnessApp.Models.General;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class ChangePassword : ContentPage
    {
        private ChangePWVM cPWVM;
        private string curPW;

        public ChangePassword(User _user)
        {
            InitializeComponent();
            cPWVM = new ChangePWVM(_user);
            Start();
            BindingContext = cPWVM;
        }

        private void Start()
        {
            Title = "Passwort ändern";
            SetNavBar();
            curPW = AllVM.Datenbank.User.GetPasswort(cPWVM.User.Nutzername);
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

        /// <summary>
        /// Prüfen und Speichern des neuen Passworts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, EventArgs e)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            string chpw = AllVM.HashPassword(cPWVM.CurPW);

            if (curPW == chpw)
            {
                if (!string.IsNullOrWhiteSpace(cPWVM.NewPW1))
                {
                    if (!string.IsNullOrWhiteSpace(cPWVM.NewPW1))
                    {
                        if (hasMinimum8Chars.IsMatch(cPWVM.NewPW1))
                        {
                            if (hasLowerChar.IsMatch(cPWVM.NewPW1))
                            {
                                if (hasUpperChar.IsMatch(cPWVM.NewPW1))
                                {
                                    if (hasNumber.IsMatch(cPWVM.NewPW1))
                                    {
                                        if (cPWVM.NewPW1 == cPWVM.NewPW2)
                                        {
                                            string hpw = AllVM.HashPassword(cPWVM.NewPW1);

                                            if (AllVM.Datenbank.User.ChangePW(cPWVM.User.Nutzername, hpw))
                                            {
                                                AllVM.User.Passwort = hpw;
                                                OnBackButtonPressed();
                                                DependencyService.Get<IMessage>().ShortAlert("Passwort erfolgreich geändert");
                                            }
                                            else
                                                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Ändern");
                                        }
                                        else
                                            DependencyService.Get<IMessage>().ShortAlert("Passwörter stimmen nicht überein");
                                    }
                                    else
                                        DependencyService.Get<IMessage>().ShortAlert("Passwort muss mindestens eine Nummer enthalten");
                                }
                                else
                                    DependencyService.Get<IMessage>().ShortAlert("Passwort muss mindestens einen Großbuchstaben enthalten");
                            }
                            else
                                DependencyService.Get<IMessage>().ShortAlert("Passwort muss mindestens einen Kleinbuchstaben enthalten");
                        }
                        else
                            DependencyService.Get<IMessage>().ShortAlert("Passwort muss mindestens 8 Zeichen lang sein");
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Passwort wiederholen!");
                }
                else
                    DependencyService.Get<IMessage>().ShortAlert("Neues Passwort muss gefüllt werden");
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Aktuelles Passwort ist nicht korrekt!");
        }

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }
    }
}