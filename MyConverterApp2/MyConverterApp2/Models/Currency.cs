using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyConverterApp2.Models
{
    public sealed class SupportedCurrenciesResponse
    {
        [JsonPropertyName("supportedCurrenciesMap")]
        public Dictionary<string, Currency>? SupportedCurrenciesMap { get; set; }
    }
    public class Currency
    {
        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; set; }

        [JsonPropertyName("currencyName")]
        public string? CurrencyName { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("countryName")]
        public string? CountryName { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("availableFrom")]
        public string? AvailableFrom { get; set; }

        [JsonPropertyName("availableUntil")]
        public string? AvailableUntil { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }
    }
}