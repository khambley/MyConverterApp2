using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyConverterApp2.Models;
using MyConverterApp2.Services;

namespace MyConverterApp2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IRateService rateService;

        [ObservableProperty]
        private Unit? unit;

        [ObservableProperty]
        ObservableCollection<string>? currencyBaseNames;
        
        [ObservableProperty]
        bool isResultLabelVisible;


        public MainViewModel(IRateService rateService)
        {
            this.rateService = rateService;
            SetCurrencyBaseNames();
            IsResultLabelVisible = false;

            Unit = new Unit();
            Unit.AutoConvertCallback = AutoConvertAsync;
        }

        private async Task AutoConvertAsync()
        {
            if (!string.IsNullOrWhiteSpace(Unit?.SelectedFromUnit) &&
                !string.IsNullOrWhiteSpace(Unit?.SelectedToUnit) &&
                !string.IsNullOrWhiteSpace(Unit?.UnitValue))
            {
                await GetRatesAsync();
            }
        }
        private void SetCurrencyBaseNames()
        {
            CurrencyBaseNames = rateService.SetBaseNames();
        }
        public string SplitBaseString(string s)
        {
            return s.Split(' ')[0];
        }

        [RelayCommand]
        public async Task ClearResultAsync()
        {
            Unit.ConversionResult = "";
            Unit.UnitValue = "";
            Unit.SelectedFromUnit = "";
            Unit.SelectedToUnit = "";
            IsResultLabelVisible = false;
        }
        [RelayCommand]
        public async Task GetRatesAsync()
		{
            var newBaseFrom = SplitBaseString(Unit?.SelectedFromUnit ?? "");

            Unit.CurrencyRate = await rateService.GetRates(newBaseFrom ?? "");

            if(Unit.CurrencyRate != null)
            {
                await ConvertRate();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Currency Rate cannot be null", "OK");
            }            
        }

        public async Task ConvertRate()
        {
            var newBaseTo = SplitBaseString(Unit?.SelectedToUnit ?? "");

            decimal convertRate = 0;

            switch (newBaseTo)
            {
                case "MXN":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.MXN != null ? Unit?.CurrencyRate?.Rate?.MXN : "");
                    break; // Curse you null safety ;)
                case "GBP":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.GBP != null ? Unit?.CurrencyRate.Rate.GBP : "");
                    break;
                case "EUR":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.EUR != null ? Unit?.CurrencyRate.Rate.EUR : "");
                    break;
                case "BTC":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.BTC != null ? Unit?.CurrencyRate.Rate.BTC : "");
                    break;
                case "CAD":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.CAD != null ? Unit?.CurrencyRate.Rate.CAD : "");
                    break;
                case "JPY":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.JPY != null ? Unit?.CurrencyRate.Rate.JPY : "");
                    break;
                case "RUB":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.RUB != null ? Unit?.CurrencyRate.Rate.RUB : "");
                    break;
                case "KRW":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.KRW != null ? Unit?.CurrencyRate.Rate.KRW : "");
                    break;
                case "USD":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.USD != null ? Unit?.CurrencyRate.Rate.USD : "");
                    break;
                default:
                    await Application.Current.MainPage.DisplayAlert("Error", "No matching currency found", "OK");
                    break;
            }
            Unit.ConversionResult = (decimal.Parse(Unit?.UnitValue) * convertRate).ToString("F2");
            IsResultLabelVisible = true;
        }
    }
}