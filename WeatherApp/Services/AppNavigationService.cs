using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using WeatherApp.ViewModels;
using WeatherApp.Views;

namespace WeatherApp.Services;

public sealed class AppNavigationService(IServiceProvider serviceProvider) : INavigationService
{
    public async Task GoToResultsAsync(CityLocation location)
    {
        var page = serviceProvider.GetRequiredService<ResultsPage>();

        if (page.BindingContext is ResultsViewModel viewModel)
        {
            await viewModel.LoadAsync(location);
        }

        var navigation = (Application.Current?.Windows.FirstOrDefault()?.Page?.Navigation)
            ?? throw new InvalidOperationException("Navigation not available.");

        await navigation.PushAsync(page);
    }

    public async Task ShowLocationPopupAsync(CityLocation location)
    {
        var popupPage = serviceProvider.GetRequiredService<LocationPopupPage>();

        if (popupPage.BindingContext is LocationPopupViewModel boundVm)
        {
            await boundVm.BindLocationAsync(location);
            boundVm.IsVisible = true;
        }

        var navigation = Application.Current?.Windows.FirstOrDefault()?.Page?.Navigation
                    ?? throw new InvalidOperationException("Navigation not available.");


        await navigation.PushModalAsync(popupPage);
    }
}
