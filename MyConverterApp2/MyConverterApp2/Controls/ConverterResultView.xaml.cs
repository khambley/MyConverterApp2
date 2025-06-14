namespace MyConverterApp2.Controls;

public partial class ConverterResultView : ContentView
{
	public ConverterResultView()
	{
		InitializeComponent();
	}
	// Is Result Visible
	public static readonly BindableProperty IsResultVisibleProperty =
		BindableProperty.Create(nameof(IsResultVisible), typeof(bool), typeof(ConverterResultView), true);

	public bool IsResultVisible
	{
		get => (bool)GetValue(IsResultVisibleProperty);
		set => SetValue(IsResultVisibleProperty, value);
	}

	// From Unit Symbol
	public static readonly BindableProperty FromSymbolProperty =
		BindableProperty.Create(nameof(FromSymbol), typeof(string), typeof(ConverterResultView), string.Empty);

	public string FromSymbol
	{
		get => (string)GetValue(FromSymbolProperty);
		set => SetValue(FromSymbolProperty, value);
	}
	
	// To Unit Symbol
    public static readonly BindableProperty ToSymbolProperty =
        BindableProperty.Create(nameof(ToSymbol), typeof(string), typeof(ConverterResultView), string.Empty);

    public string ToSymbol
    {
        get => (string)GetValue(ToSymbolProperty);
        set => SetValue(ToSymbolProperty, value);
    }
	
	// Unit Value Input
	public static readonly BindableProperty UnitValueProperty =
		BindableProperty.Create(nameof(UnitValue), typeof(double), typeof(ConverterResultView), default(double), BindingMode.TwoWay);

    public double UnitValue
    {
        get => (double)GetValue(UnitValueProperty);
        set => SetValue(UnitValueProperty, value);
    }

    // Conversion Result
    public static readonly BindableProperty ConversionResultProperty =
        BindableProperty.Create(nameof(ConversionResult), typeof(double), typeof(ConverterResultView), default(double));

    public double ConversionResult
    {
        get => (double)GetValue(ConversionResultProperty);
        set => SetValue(ConversionResultProperty, value);
    }
}