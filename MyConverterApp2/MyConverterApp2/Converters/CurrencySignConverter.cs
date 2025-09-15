using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyConverterApp2.Converters
{
    public class CurrencySignConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if((string)value == "USD")
            {
                return "USD $";
            }
            else if ((string)value == "GBP")
            {
                // Opt + 3
                return "GBP £";
            }
            else if ((string)value == "EUR")
            {
                // Opt + Shift + 2
                return "EUR €";
            }
            else if ((string)value == "BTC")
            {
                return "BTC ₿";
            }
            else if ((string)value == "MXN")
            {
                return "MXN ₱";
            }
            else if ((string)value == "CAD")
            {
                return "CAD $";
            }
            else if ((string)value == "JPY")
            {
                // Opt + y
                return "JPY ¥";
            }
            else if ((string)value == "RUB")
            {
                return "RUB ₽";
            }
            else if ((string)value == "KRW")
            {
                return "KRW ₩";
            }
            else if ((string)value == "HKD")
            {
                return "HKD $";
            }
            else
            {
                return "";
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}