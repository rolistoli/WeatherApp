using WeatherApp.Models;

namespace WeatherApp.Services.Interfaces;

public interface INavigationService
{
    Task<bool> GoToResultsAsync(CityLocation location);
    Task<bool> ShowLocationPopupAsync(CityLocation location);
    Task GoToSearchAsync();
    Task ShowSettingsAsync();
}
