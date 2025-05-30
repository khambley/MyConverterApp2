using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyConverterApp2.Models;

namespace MyConverterApp2.Services
{
    public class RateService : IRateService, IDisposable
    {
        private bool disposedValue;

        const string UriBase = "https://api.currencyfreaks.com/v2.0";

        readonly HttpClient httpClient = new()
        {
            BaseAddress = new(UriBase),
            DefaultRequestHeaders = { { "user-agent", "maui-projects-unitconverter/1.0" } }
        };

        public RateService()
        {
        }

        public async Task<CurrencyRate> GetRates(string selectedBase)
        {
            CurrencyRate result = new CurrencyRate();

            string url = $"{UriBase}/rates/latest?apikey={Settings.CurrencyApiKey}&symbols=MXN,GBP,EUR,BTC,CAD,JPY,RUB,KRW,USD&base={selectedBase}";
            try
            {

                result = await httpClient.GetFromJsonAsync<CurrencyRate>(url);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"HTTP Get failed: {ex.Message}", "OK");
            }
            return result;
        }
        public ObservableCollection<string> SetBaseNames()
        {
            return new ObservableCollection<string>()
            {
                "MXN - Mexican Peso",
                "GBP - Pound Sterling",
                "EUR - Euro",
                "BTC - Bitcoin",
                "CAD - Canadian Dollar",
                "JPY - Japanese Yen",
                "RUB - Russian Ruble",
                "KRW - S Korean Won",
                "USD - US Dollar",
            };
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    httpClient.Dispose();
                }
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}