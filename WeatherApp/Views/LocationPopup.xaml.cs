using System;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class LocationPopup : ContentView
{
    public LocationPopup()
    {
        InitializeComponent();
    }

    public LocationPopup(LocationPopupViewModel viewModel) : this()
    {
        BindingContext = viewModel;
    }

    public async Task BindLocationAsync(CityLocation loc)
    {
        if (BindingContext is LocationPopupViewModel vm)
        {
            await vm.BindLocationAsync(loc);
        }
    }

    private void OnCloseButtonClicked(object? sender, EventArgs e)
    {
        // stop propagation: mark event handled by removing focus from the map control
        // and execute CloseCommand on the VM to ensure popup closes
        if (BindingContext is LocationPopupViewModel vm)
        {
            if (vm.CloseCommand is not null && vm.CloseCommand.CanExecute(null))
            {
                vm.CloseCommand.Execute(null);
            }
            else
            {
                vm.IsVisible = false;
            }
        }
    }
}
