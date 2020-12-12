using System;
using Xamarin.Forms;

namespace FitnessApp.Models.General
{
    //Autor: NiE

    public interface IMessage
    {
        void ShortAlert(string message);
        void LongAlert(string message);
    }

    public interface ICloseApp
    {
        void Close();
    }

    public interface IImageManager
    {
        Size GetDimensionsFrom(byte[] bytes);
    }
}
