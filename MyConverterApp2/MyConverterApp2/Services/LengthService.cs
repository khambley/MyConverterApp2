using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyConverterApp2.Services
{
    public class LengthService : ILengthService
    {
        public ObservableCollection<string> SetBaseNames()
        {
            return new ObservableCollection<string>()
            {
                "Miles",
                "Yards",
                "Feet",
                "Inches",
                "Mils",
                "Nautical Miles",
                "Li",
                "Half Marathons",
                "Marathons",
                "Parsecs",
                "Milliparsecs",
                "Kilometers",
                "Meters",
                "Decimeters",
                "Centimeters",
                "Millimeters",
                "Microns"
            };
        }
        private readonly Dictionary<(string from, string to), decimal> _conversionFactors = new();

        public LengthService()
        {
            InitializeLengthConversions();
        }

        private void InitializeLengthConversions()
        {
            var metersPerUnit = new Dictionary<string, decimal>
            {
                { "miles", 1609.344m },
                { "yards", 0.9144m },
                { "feet", 0.3048m },
                { "inches", 0.0254m },
                { "mils", 0.0000254m },
                { "nautical miles", 1852m },
                { "li", 500m },
                { "half marathons", 21097.5m },
                { "marathons", 42195m },
                { "parsecs", 3.085677581e+16m },
                { "milliparsecs", 3.085677581e+13m },
                { "kilometers", 1000m },
                { "meters", 1m },
                { "decimeters", 0.1m },
                { "centimeters", 0.01m },
                { "millimeters", 0.001m },
                { "microns", 0.000001m }
            };

            // Build all (from, to) pairs
            foreach (var fromUnit in metersPerUnit.Keys)
            {
                foreach (var toUnit in metersPerUnit.Keys)
                {
                    if (fromUnit != toUnit)
                    {
                        var factor = metersPerUnit[fromUnit] / metersPerUnit[toUnit];
                        _conversionFactors[(fromUnit.ToLower(), toUnit.ToLower())] = factor;
                    }
                }
            }
        }

        public async Task<string>? Convert(string? fromUnit, string? toUnit, string? valueStr)
        {
            if (string.IsNullOrWhiteSpace(fromUnit) ||
                string.IsNullOrWhiteSpace(toUnit) ||
                !decimal.TryParse(valueStr, out var value))
                return null;

            var key = (fromUnit.ToLower(), toUnit.ToLower());
            if (_conversionFactors.TryGetValue(key, out var factor))
            {
                var result = value * factor;
                return result.ToString("F2");
            }

            return null;
        }

        public string? GetConversionSummary(string? fromUnit, string? toUnit)
        {
            if (string.IsNullOrWhiteSpace(fromUnit) || string.IsNullOrWhiteSpace(toUnit))
                return null;

            var key = (fromUnit.ToLower(), toUnit.ToLower());
            if (_conversionFactors.TryGetValue(key, out var factor))
            {
                return $"1 {fromUnit} = {factor:F5} {toUnit}";
            }

            return null;
        }
    }
}