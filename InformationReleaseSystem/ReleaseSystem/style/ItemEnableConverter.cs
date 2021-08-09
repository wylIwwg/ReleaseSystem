using ReleaseSystem.bean;
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
    public class ItemEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           /* DaysBean db = (DaysBean)value;
            string day = db.day;
            if (day != null && day.Length > 0 &&db.count<db.limit)
            {
                return true;
            }
            */

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
