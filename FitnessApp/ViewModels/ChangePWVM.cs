using System;
using System.Collections.Generic;

namespace FitnessApp
{
    //Autor: Niklas Erichsen

    public class ChangePWVM : NotifyPropertyBase
    {
        private string _curPW;
        private string _newPW1;
        private string _newPW2;

        public string CurPW { get { return _curPW; } set { OnPropertyChanged(ref _curPW, value); } }
        public string NewPW1 { get { return _newPW1; } set { OnPropertyChanged(ref _newPW1, value); } }
        public string NewPW2 { get { return _newPW2; } set { OnPropertyChanged(ref _newPW2, value); } }

        public User User { get; set; }

        public ChangePWVM(User _user)
        {
            this.User = _user;
        }
    }
}
