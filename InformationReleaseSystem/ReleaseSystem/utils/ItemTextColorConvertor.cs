using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ReleaseSystem.utils
{
    public sealed class ItemTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            SolidColorBrush scb = new SolidColorBrush();
            dynamic item = value;

            int s = item.type;
            switch (s)
            {
                case 0: scb.Color = (Color)ColorConverter.ConvertFromString("#289901");
                    break;
                default: scb.Color = (Color)ColorConverter.ConvertFromString("#FFA102");
                    break;

            }

            return scb;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
