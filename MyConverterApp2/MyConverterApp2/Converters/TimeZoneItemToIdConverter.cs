using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyConverterApp2.Converters
{
    public class TimeZoneItemToIdConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c)
    {
        // SelectedTargetTzId -> SelectedItem (TimeZoneItem)
        var id = value as string;
        if (id is null) return null;
        // ItemsSource is on the binding context (VM)
        // This converter will be called after ItemsSource is set.
        if (Application.Current?.MainPage?.BindingContext is MyConverterApp2.ViewModels.MainViewModel vm)
            return vm.TimeZones.FirstOrDefault(z => z.Id == id);
        return null;
    }

    public object ConvertBack(object value, Type t, object p, CultureInfo c)
        => value is MyConverterApp2.ViewModels.TimeZoneItem item ? item.Id : null;
    }
}