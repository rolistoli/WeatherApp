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


            var navigation = Application.Current?.Windows.FirstOrDefault()?.Page?.Navigation;

            if (navigation is null)
            {
                throw new InvalidOperationException("Navigation is not available.");
            }

            await navigation.PushAsync(page);
        }
    }
}
