using System;
using System.Collections.Generic;
using FitnessApp.Models.General;

namespace FitnessApp
{
    public class ProfilVM : NotifyPropertyBase
    {
        private User _user;
        private List<News> _fitFeed;
        private string _aboBtnText;
        private List<User> _users;
        private List<User> _userList;

        public User User { get { return _user; } set { OnPropertyChanged(ref _user, value); } }
        public List<News> FitFeed { get { return _fitFeed; } set { OnPropertyChanged(ref _fitFeed, value); } }
        public string AboBtnText { get { return _aboBtnText; } set { OnPropertyChanged(ref _aboBtnText, value); } }
        public List<User> Users { get { return _users; } set { OnPropertyChanged(ref _users, value); } }
        public List<User> UserList { get { return _userList; } set { OnPropertyChanged(ref _userList, value); } }

        public ProfilVM(User _user)
        {
            this.User = _user;
            this.FitFeed = AllVM.Datenbank.Feed.GetByUser(_user);
            this.AboBtnText = "Follow";
        }

        public ProfilVM()
        {

        }
    }
}
