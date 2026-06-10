using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.ViewModels;

public sealed partial class SearchViewModel : BaseViewModel
{
    private readonly IGeocodingService geocodingService;
    private readonly INavigationService navigationService;

    [ObservableProperty]
    private string cityName = string.Empty;

    [ObservableProperty]
    private CityLocation? selectedLocation;

    [ObservableProperty]
    private bool isEntryLoading;

    public bool HasResults => Results.Count > 0;

    public SearchViewModel(IGeocodingService geocodingService, INavigationService navigationService)
    {
        this.geocodingService = geocodingService;
        this.navigationService = navigationService;
    }

    public ObservableCollection<CityLocation> Results { get; } = new();

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(CityName):
                _ = LoadSuggestionsAsync(CityName ?? string.Empty);
                break;

            case nameof(SelectedLocation):
                if (SelectedLocation is null)
                {
                    return;
                }

                _ = HandleSelectionAsync(SelectedLocation);
                break;

            case nameof(Results):
                OnPropertyChanged(nameof(HasResults));
                break;
        }
    }

    private async Task LoadSuggestionsAsync(string searchText)
    {
        try
        {
            Results.Clear();

            var query = searchText.Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            IsEntryLoading = true;

            var locations = await geocodingService.SearchAsync(
                query,
                count: 10);

            if (locations is null || locations.Count == 0)
            {
                return;
            }

            ApplyResults(locations);
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex);
        }
        finally
        {
            IsEntryLoading = false;
        }
    }

    private async Task HandleSelectionAsync(CityLocation location)
    {
        try
        {
            IsLoading = true;

            await navigationService.GoToResultsAsync(location);
        }
        finally
        {
            IsLoading = false;
            SelectedLocation = null;
        }
    }

    public async Task HandleMapTapAsync(CityLocation location)
    {
        CityName = string.Empty;
        Results.Clear();

        if (location is null)
        {
            return;
        }

        IsLoading = true;

        try
        {
            await navigationService.ShowLocationPopupAsync(location);
        }
        finally
        {
            IsLoading = false;
        }
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
