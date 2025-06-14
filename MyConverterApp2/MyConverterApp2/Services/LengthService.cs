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
                "Mile",
                "Yard",
                "Foot",
                "Inch",
                "Mil",
                "Nautical Mile",
                "Li",
                "Half Marathon",
                "Marathon",
                "Parsec",
                "Milliparsec",
                "Kilometer",
                "Meter",
                "Decimeter",
                "Centimeter",
                "Millimeter",
                "Micron"
            };
        }
    }
}