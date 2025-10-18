using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyConverterApp2.Converters
{
    public class BoolToPinIconConverter
    {
        public ImageSource PinnedIcon { get; set; } = "ic_pin_filled.png";
        public ImageSource UnpinnedIcon { get; set; } = "ic_pin_outline.png";

        public object Convert(object value, Type t, object p, CultureInfo c)
        => value is bool b && b ? PinnedIcon : UnpinnedIcon;

        public object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }
}