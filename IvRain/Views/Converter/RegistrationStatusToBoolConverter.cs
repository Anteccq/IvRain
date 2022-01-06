using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IvRain.Models;
using Microsoft.UI.Xaml.Data;

namespace IvRain.Views.Converter
{
    public class RegistrationStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not RegistrationStatus rs)
                throw new InvalidOperationException("The value must be RegistrationStatus type.");
            return rs == RegistrationStatus.Registrable;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is not RegistrationStatus b)
                throw new InvalidOperationException("The value must be RegistrationStatus type.");
            return b != RegistrationStatus.Registrable;
        }
    }
}
