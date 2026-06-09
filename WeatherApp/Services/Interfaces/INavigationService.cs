using WeatherApp.Models;

namespace WeatherApp.Services.Interfaces;

public interface INavigationService
{
    Task GoToResultsAsync(CityLocation location);
    Task GoToResultsModalAsync(CityLocation location);
}
