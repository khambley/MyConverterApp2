using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyConverterApp2.Services
{
    public interface ILengthService
    {
        ObservableCollection<string> SetBaseNames();
        Task<string>? Convert(string? fromUnit, string? toUnit, string? valueStr);
        string? GetConversionSummary(string? fromUnit, string? toUnit);
        
    }
}