using System;
using Xamarin.Forms;
using FitnessApp.Models.General;
using Foundation;
using UIKit;

namespace FitnessApp.iOS.Helper
{
    //Autor: Niklas Erichsen

    public class ImageManagerIOS : IImageManager
    {
        /// <summary>
        /// Original-Größe des Bildes ermitteln
        /// </summary>
        public Size GetDimensionsFrom(byte[] bytes)
        {
            var data = NSData.FromArray(bytes);
            UIImage originalImage = new UIImage(data);
            return new Size(originalImage.Size.Width, originalImage.Size.Height);
        }
    }
}
