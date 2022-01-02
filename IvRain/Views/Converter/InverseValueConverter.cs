using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace IvRain.Views.Converter
{
    public class InverseValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not bool b)
                throw new InvalidOperationException("The value must be a boolean");
            return !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is not bool b)
                throw new InvalidOperationException("The value must be a boolean");
            return !b;
        }
    }
}
