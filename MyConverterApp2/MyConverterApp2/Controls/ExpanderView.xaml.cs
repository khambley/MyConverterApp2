namespace MyConverterApp2.Controls;

public partial class ExpanderView : ContentView
{
	public static readonly BindableProperty HeaderTextProperty =
		BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(ExpanderView), string.Empty);

	public static readonly BindableProperty HeaderBackgroundColorProperty =
		BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(ExpanderView), Colors.LightGray);

	public static readonly BindableProperty ExpanderContentProperty =
		BindableProperty.Create(nameof(ExpanderContent), typeof(View), typeof(ExpanderView), propertyChanged: OnContentChanged);

	public static readonly BindableProperty HeaderIconProperty =
		BindableProperty.Create(nameof(HeaderIcon), typeof(string), typeof(ExpanderView), string.Empty);

	public string HeaderText
	{
		get => (string)GetValue(HeaderTextProperty);
		set => SetValue(HeaderTextProperty, value);
	}
	public string HeaderIcon
	{
		get => (string)GetValue(HeaderIconProperty);
		set => SetValue(HeaderIconProperty, value);
	}
	public Color HeaderBackgroundColor
	{
		get => (Color)GetValue(HeaderBackgroundColorProperty);
		set => SetValue(HeaderBackgroundColorProperty, value);
	}

	public View ExpanderContent
	{
		get => (View)GetValue(ExpanderContentProperty);
		set => SetValue(ExpanderContentProperty, value);
	}
	public ExpanderView()
	{
		InitializeComponent();
	}
	private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is ExpanderView view && newValue is View newContent)
		{
			view.ExpanderContentContainer.Children.Clear();
			view.ExpanderContentContainer.Children.Add(newContent);
		}
	}
	private void OnHeaderTapped(object sender, EventArgs e)
    {
        UpArrow.IsVisible = !UpArrow.IsVisible;
        DownArrow.IsVisible = !DownArrow.IsVisible;
    }
}