using System.Collections;
using System.Windows.Input;

namespace MyConverterApp2.Controls;

public partial class ConverterEntryView : ContentView
{
    public ConverterEntryView()
    {
        InitializeComponent();
    }
    public static readonly BindableProperty UnitNameProperty =
        BindableProperty.Create(nameof(UnitName), typeof(string), typeof(ConverterEntryView), string.Empty);

    public string UnitName
    {
        get => (string)GetValue(UnitNameProperty);
        set => SetValue(UnitNameProperty, value);
    }
    // Unit Names (ItemsSource)
    public static readonly BindableProperty UnitNamesProperty =
        BindableProperty.Create(nameof(UnitNames), typeof(IEnumerable), typeof(ConverterEntryView));

    public IEnumerable UnitNames
    {
        get => (IEnumerable)GetValue(UnitNamesProperty);
        set => SetValue(UnitNamesProperty, value);
    }

    // Selected From Unit
    public static readonly BindableProperty SelectedFromUnitProperty =
        BindableProperty.Create(nameof(SelectedFromUnit), typeof(string), typeof(ConverterEntryView), default(string), BindingMode.TwoWay);

    public string SelectedFromUnit
    {
        get => (string)GetValue(SelectedFromUnitProperty);
        set => SetValue(SelectedFromUnitProperty, value);
    }

    // Selected To Unit
    public static readonly BindableProperty SelectedToUnitProperty =
        BindableProperty.Create(nameof(SelectedToUnit), typeof(string), typeof(ConverterEntryView), default(string), BindingMode.TwoWay);

    public string SelectedToUnit
    {
        get => (string)GetValue(SelectedToUnitProperty);
        set => SetValue(SelectedToUnitProperty, value);
    }

    // Unit Value Input
    public static readonly BindableProperty UnitValueProperty =
        BindableProperty.Create(nameof(UnitValue), typeof(string), typeof(ConverterEntryView), default(string), BindingMode.TwoWay);

    public string UnitValue
    {
        get => (string)GetValue(UnitValueProperty);
        set => SetValue(UnitValueProperty, value);
    }
    // Command for swap
    public ICommand SwapUnitsCommand => new Command(() =>
    {
        var temp = SelectedFromUnit;
        SelectedFromUnit = SelectedToUnit;
        SelectedToUnit = temp;
    });
}