using WeatherApp.Views;

namespace WeatherApp.Services;

public static class ErrorPopupService
{
    public static Task ShowErrorAsync(string message)
    {
        var tcsReturn = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var current = Application.Current?.Windows.FirstOrDefault();
            if (current is null)
            {
                tcsReturn.TrySetResult(true);
                return;
            }

            var page = current.Page;
            if (page is null)
            {
                tcsReturn.TrySetResult(true);
                return;
            }

            var popup = new ErrorPopup("Error", message);
            try
            {
                await page.Navigation.PushModalAsync(popup, false);
            }
            catch
            {
                tcsReturn.TrySetResult(true);
                return;
            }

            // wait for the popup's dismiss signal, then pop the modal and complete
            try
            {
                await popup.DismissTask;
            }
            catch
            {
                // ignore
            }

            try
            {
                await page.Navigation.PopModalAsync(false);
            }
            catch
            {
                // ignore
            }

            tcsReturn.TrySetResult(true);
        });

        return tcsReturn.Task;
    }
}
