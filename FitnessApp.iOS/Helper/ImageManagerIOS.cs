using Xamarin.Forms;
using Foundation;
using UIKit;

namespace FitnessApp.iOS
{
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
