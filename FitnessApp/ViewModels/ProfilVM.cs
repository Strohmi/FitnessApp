using System;
using FitnessApp.Models;

namespace FitnessApp.ViewModels
{
    public class ProfilVM
    {
        public User User { get; set; }

        public ProfilVM()
        {
            User = new User()
            {
                Vorname = "Niklas",
                Nachname = "Erichsen",
                Geburtsdatum = new DateTime(1996, 02, 09)
            };
        }
    }
}
