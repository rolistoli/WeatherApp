using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class LocationPopup : ContentView
{
    public LocationPopup(LocationPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public async Task BindLocationAsync(CityLocation loc)
    {
        if (BindingContext is LocationPopupViewModel vm)
        {
            await vm.BindLocationAsync(loc);
        }
    }
}
