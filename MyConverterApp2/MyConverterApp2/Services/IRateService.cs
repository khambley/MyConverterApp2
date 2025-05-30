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
        public Task<CurrencyRate> GetRates(string selectedBase);
        public ObservableCollection<string> SetBaseNames();
    }
}