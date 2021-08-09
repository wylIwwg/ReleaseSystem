using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using ReleaseSystem.bean;

namespace ReleaseSystem.style
{
    public class ItemDateBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush scb = new SolidColorBrush();
            scb.Color = (Color)ColorConverter.ConvertFromString("#757575");
          /*  string number = (value).number;
            string[] nums = number.Split('/');
            if (nums != null && nums.Length == 2)
            {
                if ((Int32.Parse(nums[0]) == Int32.Parse(nums[1])))
                {
                    scb.Color = (Color)ColorConverter.ConvertFromString("#FF0000");
                }
            }*/
            return scb;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
