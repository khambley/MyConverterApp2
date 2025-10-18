using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyConverterApp2.Converters
{
  public class DateTimeToTimeSpanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is DateTime dt ? dt.TimeOfDay : TimeSpan.Zero;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var now = DateTime.Now;
        var ts = value is TimeSpan t ? t : TimeSpan.Zero;
        // keep the VM’s LocalTime date, only replace time-of-day
        if (parameter is DateTime baseDate) // not used here, but pattern shows how to pass if needed
            return new DateTime(baseDate.Year, baseDate.Month, baseDate.Day).Add(ts);
        return new DateTime(now.Year, now.Month, now.Day).Add(ts);
    }
  }
}