using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyConverterApp2.Converters
{
    public class StringNotNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c) => !string.IsNullOrWhiteSpace(value as string);
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }
}