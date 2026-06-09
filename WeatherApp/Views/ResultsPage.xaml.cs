using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class ResultsPage : ContentPage
{
    public ResultsPage(ResultsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        return base.OnBackButtonPressed();
    }
}
