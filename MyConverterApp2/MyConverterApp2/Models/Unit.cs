using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyConverterApp2.Models
{
    public partial class Unit : ObservableObject
    {
        [ObservableProperty]
        string? unitName;
        [ObservableProperty]
        string? sourceUnit;
        [ObservableProperty]
        string? targetUnit;
        [ObservableProperty]
        string? sourceValue;
        [ObservableProperty]
        string? targetValue;
        



    }
}