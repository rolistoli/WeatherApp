using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeatherApp.Helpers;

namespace WeatherApp.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    private bool _isLoading;
    private string _errorMessage = string.Empty;
    private string? _localizedErrorKey;
    private bool _isSettingLocalizedError;

    protected BaseViewModel()
    {
        // AppSettings.LanguageChanged += OnLanguageChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (SetProperty(ref _isLoading, value))
            {
                OnPropertyChanged(nameof(IsNotLoading));
            }
        }
    }

    public bool IsNotLoading => !IsLoading;

    protected string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (!_isSettingLocalizedError)
            {
                _localizedErrorKey = null;
            }

            if (SetProperty(ref _errorMessage, value))
            {
                OnPropertyChanged(nameof(HasError));
            }
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void ClearError()
    {
        _localizedErrorKey = null;
        ErrorMessage = string.Empty;
    }

    protected void SetLocalizedError(string key)
    {
        _localizedErrorKey = key;
        _isSettingLocalizedError = true;
        // ErrorMessage = LocalizedStrings.Current[key];
        _isSettingLocalizedError = false;
    }

    protected virtual void RefreshLocalizedState()
    {
        if (!string.IsNullOrWhiteSpace(_localizedErrorKey))
        {
            SetLocalizedError(_localizedErrorKey);
        }
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        if (MainThread.IsMainThread)
        {
            RefreshLocalizedState();
            return;
        }

        MainThread.BeginInvokeOnMainThread(RefreshLocalizedState);
    }
}
