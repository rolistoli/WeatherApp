using WeatherApp.Helpers;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using WeatherApp.ViewModels;
using WeatherApp.Views;

namespace WeatherApp.Services;

public sealed class AppNavigationService(IServiceProvider serviceProvider) : INavigationService
{
    public async Task GoToResultsAsync(CityLocation location)
    {
        try
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
        catch (Exception ex)
        {
            throw new ApiException("An error occurred.", ex);
        }
    }

    public async Task ShowLocationPopupAsync(CityLocation location)
    {
        try
        {
            var popupView = serviceProvider.GetRequiredService<LocationPopup>();

            if (popupView.BindingContext is LocationPopupViewModel viewModel)
            {
                await viewModel.BindLocationAsync(location);
            }

            var navigation = Application.Current?.Windows.FirstOrDefault()?.Page?.Navigation
                        ?? throw new InvalidOperationException("Navigation not available.");

            // Build a transparent modal page that hosts the popup view at the bottom
            var modal = new ContentPage
            {
                BackgroundColor = Colors.Transparent,
                Content = new Grid
                {
                    BackgroundColor = Colors.Transparent,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    Children =
                {
                    new BoxView { BackgroundColor = Color.FromArgb("#80000000"), HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill },
                    popupView
                }
                }
            };

            await navigation.PushModalAsync(modal, false);
        }
        catch (Exception ex)
        {
            throw new ApiException("An error occurred.", ex);
        }
    }
}
