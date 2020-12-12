using System;
using System.Drawing;
using Android.Graphics;
using FitnessApp.Models.General;

namespace FitnessApp.Droid.Helper
{
    //Autor: NiE

    public class ImageManagerDROID : IImageManager
    {
        public Xamarin.Forms.Size GetDimensionsFrom(byte[] bytes)
        {
            Android.Graphics.Bitmap originalImage = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            return new Xamarin.Forms.Size(originalImage.Width, originalImage.Height);
        }
    }
}
