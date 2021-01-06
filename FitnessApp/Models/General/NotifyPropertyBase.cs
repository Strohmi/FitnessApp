using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitnessApp
{
    //Autor: Niklas Erichsen

    public class NotifyPropertyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Speichern einer Variablen, nachdem sie geändert wurde
        /// </summary>
        public void OnPropertyChanged<T>(ref T variable, T value, [CallerMemberName] string propertyName = null)
        {
            variable = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}