using System;
using System.IO;
using Xamarin.Forms;

namespace FitnessApp.Models.General
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
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

    public class FitFeedTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate FotoTemplate { get; set; }

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
