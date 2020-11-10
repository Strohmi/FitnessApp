using System;

namespace FitnessApp.Models.General
{
    public interface IMessage
    {
        void ShortAlert(string message);
        void LongAlert(string message);
    }

    public interface ICloseApp
    {
        void Close();
    }
}
