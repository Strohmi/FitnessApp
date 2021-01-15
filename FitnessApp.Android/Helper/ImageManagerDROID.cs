using Android.Graphics;

namespace FitnessApp.Droid
{
    public class ImageManagerDROID : IImageManager
    {
        /// <summary>
        /// Original-Größe des Bildes ermitteln
        /// </summary>
        public Xamarin.Forms.Size GetDimensionsFrom(byte[] bytes)
        {
            Android.Graphics.Bitmap originalImage = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            return new Xamarin.Forms.Size(originalImage.Width, originalImage.Height);
        }
    }
}
