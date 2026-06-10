namespace WeatherApp.Views;

public partial class ErrorPopup : ContentPage
{
    private readonly string _titleText;
    private readonly string _messageText;
    private readonly TaskCompletionSource<bool> _tcs;

    public ErrorPopup(string title, string message)
    {
        InitializeComponent();
        _titleText = title;
        _messageText = message;
        _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        TitleLabel.Text = _titleText;
        MessageLabel.Text = _messageText;
        IconLabel.Text = "⚠";
    }

    private void OnBackgroundTapped(object? sender, EventArgs e)
    {
        try
        {
            _tcs.TrySetResult(true);
        }
        catch
        {
            // ignore
        }
    }

    public Task DismissTask => _tcs.Task;
}
