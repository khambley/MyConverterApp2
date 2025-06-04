using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyConverterApp2.Models
{
    public partial class Unit : ObservableObject
    {
        [ObservableProperty]
        string? unitName;

        [ObservableProperty]
        string? unitValue;

        [ObservableProperty]
        string? fromUnit;

        [ObservableProperty]
        string? toUnit;

        [ObservableProperty]
        string? selectedFromUnit;

        [ObservableProperty]
        string? selectedToUnit;

        [ObservableProperty]
        string? sourceValue;

        [ObservableProperty]
        string? targetValue;

        [ObservableProperty]
        private CurrencyRate? currencyRate;

        [ObservableProperty]
        string? convertionRate;

        [ObservableProperty]
        string? conversionResult;

        // ViewModel callback
        public Func<Task>? AutoConvertCallback { get; set; }

        partial void OnSelectedFromUnitChanged(string value)
        {
            AutoConvertCallback?.Invoke();
        }

        partial void OnSelectedToUnitChanged(string value)
        {
            AutoConvertCallback?.Invoke();
        }

        partial void OnUnitValueChanged(string value)
        {
            AutoConvertCallback?.Invoke();
        }

        



    }
}