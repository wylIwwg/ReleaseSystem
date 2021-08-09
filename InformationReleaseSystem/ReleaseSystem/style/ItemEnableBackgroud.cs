using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ReleaseSystem.style
{
    public class ItemEnableBackgroud : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush scb = new SolidColorBrush();
            if ((bool)value)
                scb.Color = (Color)ColorConverter.ConvertFromString("#cdcdcd");
            else
                scb.Color = (Color)ColorConverter.ConvertFromString("#FFFFFFFF"); ;
            return scb;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
