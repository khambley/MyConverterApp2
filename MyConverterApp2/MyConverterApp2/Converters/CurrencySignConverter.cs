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
            if((string)value == "USD - US Dollar")
            {
                return "USD $";
            }
            else if ((string)value == "GBP - Pound Sterling")
            {
                // Opt + 3
                return "GBP £";
            }
            else if ((string)value == "EUR - Euro")
            {
                // Opt + Shift + 2
                return "EUR €";
            }
            else if ((string)value == "BTC - Bitcoin")
            {
                return "BTC ₿";
            }
            else if ((string)value == "MXN - Mexican Peso")
            {
                return "MXN ₱";
            }
            else if ((string)value == "CAD - Canadian Dollar")
            {
                return "CAD $";
            }
            else if ((string)value == "JPY - Japanese Yen")
            {
                // Opt + y
                return "JPY ¥";
            }
            else if ((string)value == "RUB - Russian Ruble")
            {
                return "RUB ₽";
            }
            else if ((string)value == "KRW - S Korean Won")
            {
                return "KRW ₩";
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