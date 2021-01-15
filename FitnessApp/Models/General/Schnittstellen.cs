using Xamarin.Forms;

namespace FitnessApp
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

    public interface IImageManager
    {
        Size GetDimensionsFrom(byte[] bytes);
    }
}
