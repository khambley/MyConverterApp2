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
        string? lengthUnitValue;

        [ObservableProperty]
        string? selectedFromUnit;

        [ObservableProperty]
        string? selectedToUnit;

        [ObservableProperty]
        string? lengthSelectedFromUnit;

        [ObservableProperty]
        string? lengthSelectedToUnit;

        [ObservableProperty]
        private CurrencyRate? currencyRate;

        [ObservableProperty]
        string? convertionRate;

        [ObservableProperty]
        string? lengthConversionResult;

        // ViewModel currency callback
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

        // ViewModel length callback
        public Func<Task>? LengthAutoConvertCallback { get; set; }
        partial void OnLengthSelectedFromUnitChanged(string value)
        {
            LengthAutoConvertCallback?.Invoke();
        }

        partial void OnLengthSelectedToUnitChanged(string value)
        {
            LengthAutoConvertCallback?.Invoke();
        }

        partial void OnLengthUnitValueChanged(string value)
        {
            LengthAutoConvertCallback?.Invoke();
        }



        



    }
}