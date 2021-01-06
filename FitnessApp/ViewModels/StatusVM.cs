using System;

namespace FitnessApp
{
    //Autor: Niklas Erichsen

    public class StatusVM : NotifyPropertyBase
    {
        private Status _status;
        public Status Status { get { return _status; } set { OnPropertyChanged(ref _status, value); } }

        public StatusVM()
        {
            Status = new Status();
        }
    }
}
