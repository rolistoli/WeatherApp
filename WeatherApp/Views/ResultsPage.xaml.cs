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
}
