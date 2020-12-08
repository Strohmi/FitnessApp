using System;
using System.Collections.Generic;
using FitnessApp.Models;

namespace FitnessApp.ViewModels
{
    public class SearchVM : NotifyPropertyBase
    {
        private List<User> _users;
        private List<User> _usersResult;

        private List<Trainingsplan> _tlist;
        private List<Trainingsplan> _tListResult;

        private List<Ernährungsplan> _elist;
        private List<Ernährungsplan> _eListResult;

        public List<User> Users { get { return _users; } set { OnPropertyChanged(ref _users, value); } }
        public List<User> UsersResult { get { return _usersResult; } set { OnPropertyChanged(ref _usersResult, value); } }

        public List<Trainingsplan> TPläne { get { return _tlist; } set { OnPropertyChanged(ref _tlist, value); } }
        public List<Trainingsplan> TPläneResult { get { return _tListResult; } set { OnPropertyChanged(ref _tListResult, value); } }

        public List<Ernährungsplan> EPläne { get { return _elist; } set { OnPropertyChanged(ref _elist, value); } }
        public List<Ernährungsplan> EPläneResult { get { return _eListResult; } set { OnPropertyChanged(ref _eListResult, value); } }
    }
}
