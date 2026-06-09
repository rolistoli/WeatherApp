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
    // removed isMapBusy: use IsNavigating for global busy state
    private bool isNavigating;

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

            _ = HandleSelectionAsync(value);
        }
    }

    public bool HasResults
    {
        get => hasResults;
        set => SetProperty(ref hasResults, value);
    }

    // IsMapBusy removed; use IsNavigating only

    public bool IsNavigating
    {
        get => isNavigating;
        set => SetProperty(ref isNavigating, value);
    }

    private async Task LoadSuggestionsAsync(string searchText)
    {
        ClearError();

        var query = searchText.Trim();

        // if the query is empty, do not call the geocoding service or show an error;
        // simply hide the results dropdown
        if (string.IsNullOrWhiteSpace(query))
        {
            HasResults = false;
            Results.Clear();
            IsLoading = false;
            return;
        }

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

        // Keep existing behavior for other callers. Navigation from map taps will use OpenDetailsAsync instead.
        return navigationService.GoToResultsAsync(location);
    }

    private bool _isSelecting;

    private async Task HandleSelectionAsync(CityLocation location)
    {
        if (_isSelecting) return;

        _isSelecting = true;
        try
        {
            // show blocking navigation UI (separate from search loading)
            IsNavigating = true;

            await navigationService.GoToResultsAsync(location);
        }
        finally
        {
            IsNavigating = false;
            _isSelecting = false;
            // clear selection on UI thread
            SelectedLocation = null;
        }
    }

    // Called by UI (for example a popup) to navigate to details for a location
    public Task OpenDetailsAsync(CityLocation location)
    {
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
