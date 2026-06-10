using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class LocationPopup : ContentView
{
    public LocationPopup(LocationPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }   
}
