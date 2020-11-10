using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitnessApp
{
    public class NotifyPropertyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged<T>(ref T variable, T value, [CallerMemberName] string propertyName = null)
        {
            variable = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}