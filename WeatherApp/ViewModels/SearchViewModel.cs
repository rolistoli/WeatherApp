using System.Collections.ObjectModel;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using WeatherApp.Views;

namespace WeatherApp.ViewModels;

public sealed class SearchViewModel : BaseViewModel
{
    private readonly IGeocodingService geocodingService;
    private readonly INavigationService navigationService;

    private string cityName = string.Empty;
    private CityLocation? selectedLocation;
    private bool hasResults;

    public SearchViewModel(IGeocodingService geocodingService, INavigationService navigationService)
    {
        this.geocodingService = geocodingService;
        this.navigationService = navigationService;
    }

    public string CityName
    {
        get => cityName;
        set
        {
            if (SetProperty(ref cityName, value ?? string.Empty))
            {
                _ = LoadSuggestionsAsync(cityName);
            }
        }
    }

    public ObservableCollection<CityLocation> Results { get; } = [];

    public CityLocation? SelectedLocation
    {
        get => selectedLocation;
        set
        {
            if (!SetProperty(ref selectedLocation, value) || value is null)
            {
                return;
            }

            _ = SelectLocationAsync(value);
        }
    }

    public bool HasResults
    {
        get => hasResults;
        set => SetProperty(ref hasResults, value);
    }

    private async Task LoadSuggestionsAsync(string searchText)
    {
        ClearError();

        var query = searchText.Trim();

        IsLoading = true;

        try
        {

            var locations = await geocodingService.SearchAsync(
                query,
                count: 10);

            if (locations is null || locations.Count == 0)
            {
                HasResults = false;
                Results.Clear();
                return;
            }

            ApplyResults(locations);

            HasResults = true;
        }
        catch (Exception ex)
        {
            HasResults = false;
            Results.Clear();

            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private Task SelectLocationAsync(CityLocation? location)
    {
        if (location is null)
        {
            return Task.CompletedTask;
        }

        return navigationService.GoToResultsAsync(location);
    }
    private void ApplyResults(IReadOnlyList<CityLocation> results)
    {
        for (var index = 0; index < results.Count; index++)
        {
            if (index >= Results.Count)
            {
                Results.Add(results[index]);
                continue;
            }

            if (Results[index].Id != results[index].Id)
            {
                Results[index] = results[index];
            }
        }

        while (Results.Count > results.Count)
        {
            Results.RemoveAt(Results.Count - 1);
        }
    }
}
