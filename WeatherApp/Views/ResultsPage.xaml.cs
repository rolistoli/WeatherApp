using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class ResultsPage : ContentPage
{
    public ResultsPage(ResultsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public ResultsPage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        // let navigation stack handle back by default
        return base.OnBackButtonPressed();
    }
}
