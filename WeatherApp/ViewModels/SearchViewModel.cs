using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.ViewModels;

public sealed class SearchViewModel : BaseViewModel
{
    private readonly IGeocodingService geocodingService;
    private readonly INavigationService navigationService;
    private string cityName = string.Empty;
    private CityLocation? selectedLocation;
    private bool hasResults;
    // private CancellationTokenSource? suggestionsCancellationTokenSource;

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
            SelectedLocation = null;
        }
    }

    public bool HasResults
    {
        get => hasResults;
        set => SetProperty(ref hasResults, value);
    }

    private async Task LoadSuggestionsAsync(string searchText)
    {
        // suggestionsCancellationTokenSource?.Cancel();
        // suggestionsCancellationTokenSource?.Dispose();
        // suggestionsCancellationTokenSource = null;

        var query = searchText.Trim();
        ClearError();

        if (query.Length < 3)
        {
            HasResults = false;
            Results.Clear();
            IsLoading = false;
            return;
        }

        // var cancellationTokenSource = new CancellationTokenSource();
        // suggestionsCancellationTokenSource = cancellationTokenSource;
        // var cancellationToken = cancellationTokenSource.Token;

        IsLoading = true;

        try
        {
            // await Task.Delay(350, cancellationToken);

            var locations = await geocodingService.SearchAsync(
                query,
                count: 10,
                cancellationToken: new CancellationToken());

            // if (cancellationToken.IsCancellationRequested || query != CityName.Trim())
            // {
            //     return;
            // }

            if (locations.Count == 0)
            {
                HasResults = false;
                Results.Clear();
                SetLocalizedError("SearchNoSuggestions");
                return;
            }

            ApplyResults(locations);
            HasResults = true;
        }
        catch (HttpRequestException exception)
        {
            SetLocalizedError(exception.StatusCode is HttpStatusCode.BadRequest
                ? "SearchBadRequest"
                : "SearchConnectionError");
        }
        catch (TaskCanceledException)
        {
            SetLocalizedError("RequestTimeout");
        }
        catch (JsonException)
        {
            SetLocalizedError("SearchInvalidApiResponse");
        }
        catch (InvalidOperationException)
        {
            SetLocalizedError("SearchUnexpectedApiResponse");
        }
        finally
        {
            // if (suggestionsCancellationTokenSource == cancellationTokenSource)
            {
                IsLoading = false;
            }
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
