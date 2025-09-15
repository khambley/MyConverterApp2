using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyConverterApp2.Models;

namespace MyConverterApp2.Services
{
    public interface IRateService
    {
        Task<CurrencyRate> GetRates(string selectedBase);
        Task<ObservableCollection<Currency>> SetBaseNames();
        string? GetConversionSummary(CurrencyRate? rate, string? fromUnit, string? toUnit);
    }
}