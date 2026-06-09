using CommunityToolkit.Mvvm.ComponentModel;


namespace WeatherApp.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    private bool _isLoading;
    private string _errorMessage = string.Empty;

    protected BaseViewModel()
    {
    }

    public bool IsLoading
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

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (SetProperty(ref _errorMessage, value))
            {
                OnPropertyChanged(nameof(HasError));
            }
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
  

    protected void ClearError()
    {
        ErrorMessage = string.Empty;
    }
}
