using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ManagementCoach
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return value;
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");  
            return double.Parse(value.ToString()).ToString("#,###", cul.NumberFormat) + " VND";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return value;
            int x = int.Parse(value.ToString());
            var hour = (x / 60).ToString();
            var minute = (x % 60).ToString();
            if (hour.Length == 1) hour = "0" + hour;
            if (minute.Length == 1) minute = "0" + minute;
            return hour + ":" + minute;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ValueTimeConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return value;
            int x = int.Parse(value.ToString());
            var hour = (x / 60).ToString() + "h";
            var minute = (x % 60).ToString()+ "m";
            return hour + " " + minute;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
