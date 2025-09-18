using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MyConverterApp2.Models;

namespace MyConverterApp2.Services
{
    public class RateService : IRateService, IDisposable
    {
        private bool disposedValue;

        const string UriBase = "https://api.currencyfreaks.com/v2.0";

        string symbols = "BTC,CAD,EUR,GBP,HKD,JPY,KRW,MXN,RUB,USD,";


        readonly HttpClient httpClient = new()
        {
            BaseAddress = new(UriBase),
            DefaultRequestHeaders = { { "user-agent", "maui-projects-unitconverter/1.0" } }
        };

        public RateService()
        {
        }

        public string? GetConversionSummary(CurrencyRate? rate, string? fromUnit, string? toUnit)
        {
            if (rate?.Rate is null || string.IsNullOrWhiteSpace(fromUnit) || string.IsNullOrWhiteSpace(toUnit))
                return null;

            var from = SplitBaseCode(fromUnit);
            var to   = SplitBaseCode(toUnit);

            if (TryGetRate(rate, to, out var factor))
            {
                // If your model has a date/timestamp, use it; otherwise fall back to now
                var asOf = DateTime.UtcNow;
                return $"1 {from} = {factor:F4} {to} (as of {asOf:MMM dd, yyyy})";
            }

            return null;
        }     

        private static string SplitBaseCode(string s) => s?.Split(' ')[0] ?? string.Empty;

        /// <summary>Pulls a decimal rate out of the strongly-typed properties.</summary>
        private static bool TryGetRate(CurrencyRate rate, string code, out decimal value)
        {
            value = 0m;

            // Map the few you support. (Matches your 'symbols' list.)
            string? raw = code.ToUpper() switch
            {
                "MXN" => rate.Rate.MXN,
                "GBP" => rate.Rate.GBP,
                "EUR" => rate.Rate.EUR,
                "BTC" => rate.Rate.BTC,
                "CAD" => rate.Rate.CAD,
                "JPY" => rate.Rate.JPY,
                "RUB" => rate.Rate.RUB,
                "KRW" => rate.Rate.KRW,
                "USD" => rate.Rate.USD,
                "HKD" => rate.Rate.HKD,
                _ => null
            };

            return decimal.TryParse(raw, out value);
        }

        public async Task<CurrencyRate> GetRates(string selectedBase)
        {
            CurrencyRate result = new CurrencyRate();

            string url = $"{UriBase}/rates/latest?apikey={Settings.CurrencyApiKey}&symbols={symbols}&base={selectedBase}";
            try
            {

                result = await httpClient.GetFromJsonAsync<CurrencyRate>(url);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"HTTP GetRates failed: {ex.Message}", "OK");
            }
            return result;
        }
        public async Task<List<Currency>> GetSupportedCurrencies()
        {
            var url = "https://api.currencyfreaks.com/v2.0/supported-currencies";

            try
            {
                var resp = await httpClient.GetFromJsonAsync<SupportedCurrenciesResponse>(
                    url,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Flatten the map to a list
                return resp?.SupportedCurrenciesMap?.Values.ToList() ?? new List<Currency>();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"HTTP GetSupportedCurrencies failed: {ex.Message}",
                    "OK");
                return new List<Currency>();
            }
        }
        public async Task<ObservableCollection<Currency>> SetBaseNames()
        {
            var supportedCurrencies = await GetSupportedCurrencies();        

            var desiredCodes = new HashSet<string>(
                symbols.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                StringComparer.OrdinalIgnoreCase
            );

            var filteredByName = new ObservableCollection<Currency>(
                supportedCurrencies
                    .Where(c => !string.IsNullOrWhiteSpace(c.CurrencyCode) &&
                                desiredCodes.Contains(c.CurrencyCode))
                    .OrderBy(c => c.CurrencyName)      // or .OrderBy(c => c.CurrencyCode)
                    .ThenBy(c => c.CurrencyCode)
            );
            return filteredByName;

            // return new ObservableCollection<string>()
            // {
            //     "MXN - Mexican Peso",
            //     "GBP - Pound Sterling",
            //     "EUR - Euro",
            //     "BTC - Bitcoin",
            //     "CAD - Canadian Dollar",
            //     "JPY - Japanese Yen",
            //     "RUB - Russian Ruble",
            //     "KRW - S Korean Won",
            //     "USD - US Dollar",
            // };
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