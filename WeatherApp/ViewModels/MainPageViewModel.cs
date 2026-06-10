using CommunityToolkit.Mvvm.Input;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.ViewModels;

public sealed partial class MainPageViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    public MainPageViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }


    [RelayCommand]
    private async Task NavigateToSearch()
    {
        try
        {
            IsLoading = true;
            await _navigationService.GoToSearchAsync();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToSettings()
    {
        try
        {
            IsLoading = true;
            await _navigationService.ShowSettingsAsync();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
