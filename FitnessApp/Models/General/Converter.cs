using System;
using System.IO;
using Xamarin.Forms;

namespace FitnessApp
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        /// <summary>
        /// Aus byte[] eine ImageSource konvertieren
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;
                var stream = new MemoryStream(imageAsBytes);
                retSource = ImageSource.FromStream(() => stream);
            }
            return retSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageHeightConverter : IValueConverter
    {
        /// <summary>
        /// Höhe eines Bildes ermitteln
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double height = -1;
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;
                var size = DependencyService.Get<IImageManager>().GetDimensionsFrom(imageAsBytes);

                double curWidth = Application.Current.MainPage.Width - 20 - 10 - 20; //Padding ListView, Margin Frame, Padding Frame
                height = size.Height * (curWidth / size.Width);
            }
            return height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageWidthConverter : IValueConverter
    {
        /// <summary>
        /// Breite eines Bildes ermitteln
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double width = -1;
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;
                var size = DependencyService.Get<IImageManager>().GetDimensionsFrom(imageAsBytes);

                double curHeight = Application.Current.MainPage.Height;
                width = size.Width * (curHeight / size.Height);
            }
            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FitFeedTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate FotoTemplate { get; set; }

        /// <summary>
        /// Entsprechendes Template bereitstellen
        /// </summary>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            News news = (item as News);
            if (news.IsFoto)
            {
                return FotoTemplate;
            }
            else
            {
                return TextTemplate;
            }
        }
    }
}
