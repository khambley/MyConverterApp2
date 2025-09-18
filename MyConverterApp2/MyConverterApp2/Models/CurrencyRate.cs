using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyConverterApp2.Models
{
    public class CurrencyRate
    {
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("base")]
        public string? BaseUnit { get; set; }

        [JsonPropertyName("rates")]
        public Rates? Rate { get; set; }
    }
    public class Rates
    {
        public string? MXN { get; set; }
        public string? GBP { get; set; }
        public string? EUR { get; set; }
        public string? BTC { get; set; }
        public string? CAD { get; set; }
        public string? JPY { get; set; }
        public string? RUB { get; set; }
        public string? KRW { get; set; }
        public string? USD { get; set; }
        public string? HKD { get; set; }

        //MXN,GBP,EUR,BTC,CAD,JPY,RUB,KRD,USD
    }
}