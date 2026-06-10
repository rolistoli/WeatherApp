using CommunityToolkit.Mvvm.ComponentModel;
using WeatherApp.Services;

namespace WeatherApp.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    private bool isLoading;

    protected BaseViewModel()
    {
    }

    public bool IsNotLoading => !IsLoading;

    protected Task ShowErrorAsync(Exception ex)
    {
        if (ex is null)
        {
            return Task.CompletedTask;
        }

        return ErrorPopupService.ShowErrorAsync(ex.Message);
    }
}
