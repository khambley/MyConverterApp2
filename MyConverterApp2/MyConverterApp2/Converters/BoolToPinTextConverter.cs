using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyConverterApp2.Converters
{
    public class BoolToPinTextConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c)
        => value is bool b && b ? "Unpin" : "Pin";
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }
}