using System;

namespace FitnessApp
{
    public class ProfilVM : NotifyPropertyBase
    {
        private User _user;
        public User User { get { return _user; } set { OnPropertyChanged(ref _user, value); } }

        public ProfilVM()
        {

        }
    }
}
