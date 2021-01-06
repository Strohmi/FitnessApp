using System;
using System.Collections.Generic;
using System.IO;
using FitnessApp.Models;
using FitnessApp.Models.General;
using Xamarin.Forms;

namespace FitnessApp
{
    //Autor: Niklas Erichsen

    public class ProfilShowVM : NotifyPropertyBase
    {
        private User _user;
        private List<News> _fitFeed;
        private string _aboBtnText;

        public User User { get { return _user; } set { OnPropertyChanged(ref _user, value); } }
        public List<News> FitFeed { get { return _fitFeed; ; } set { OnPropertyChanged(ref _fitFeed, value); } }
        public string AboBtnText { get { return _aboBtnText; ; } set { OnPropertyChanged(ref _aboBtnText, value); } }

        public ProfilShowVM(User _user)
        {
            this.User = _user;
        }

        public ProfilShowVM()
        {

        }
    }

    public class ProfilEditVM
    {
        public User User { get; set; }

        public ProfilEditVM(User _user)
        {
            this.User = _user;
        }
    }
}
