using epj.Expander.Maui;
using MyConverterApp2.ViewModels;

namespace MyConverterApp2;

public partial class MainPage : ContentPage
{
    readonly MainViewModel viewModel;
    public MainPage(MainViewModel viewModel)
    {
        this.viewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
        
    }
	 protected override void OnAppearing()
    {
        base.OnAppearing();
        if (AccordionLayout.Children[0] is Expander)
        {
            var firstExpander = (Expander)AccordionLayout.Children[0];
            firstExpander.IsExpanded = true;
        }
    }
	void Expander_OnHeaderTapped(System.Object sender, epj.Expander.Maui.ExpandedEventArgs e)
    {
        if (sender is not Expander expander)
        {
            return;
        }

        var children = expander.GetVisualTreeDescendants();
        var downArrowLabel = (Label)children[5];
        var upArrowLabel = (Label)children[6];
        if (downArrowLabel.IsVisible)
        {
            downArrowLabel.IsVisible = false;
            upArrowLabel.IsVisible = true;
        }
        else
        {
            downArrowLabel.IsVisible = true;
            upArrowLabel.IsVisible = false;
        }


        // This is optional. This is to prevent other expanders to be opened when the active one is opened. KLH
        foreach (var child in AccordionLayout.Children)
        {
            if (child is not Expander other)
            {
                continue;

            }

            if (other != expander)
            {
                var otherChildren = other.GetVisualTreeDescendants();
                var otherDownArrowLabel = (Label)otherChildren[5];
                var otherUpArrowLabel = (Label)otherChildren[6];

                // other expanders are collapsed
                otherDownArrowLabel.IsVisible = true;
                otherUpArrowLabel.IsVisible = false;
                other.IsExpanded = false;
            }
        }
    }
}

