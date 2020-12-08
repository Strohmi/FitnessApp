using System;
namespace FitnessApp.ViewModels
{
    public class RegisterVM : NotifyPropertyBase
    {
        private User _user;
        private string _pw;
        private string _pw2;

        public User User { get { return _user; } set { OnPropertyChanged(ref _user, value); } }
        public string PW { get { return _pw; } set { OnPropertyChanged(ref _pw, value); } }
        public string PW2 { get { return _pw2; } set { OnPropertyChanged(ref _pw2, value); } }

        public RegisterVM()
        {
            this.User = new User();
        }
    }
}
