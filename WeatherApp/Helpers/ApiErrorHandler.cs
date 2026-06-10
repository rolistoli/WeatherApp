using System.Text;

namespace WeatherApp.Helpers;

public static class ApiErrorHandler
{
    public static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        string? content = null;
        try
        {
            // Try read as string first
            content = await response.Content.ReadAsStringAsync();
        }
        catch
        {
            // ignore
        }

        var message = new StringBuilder();
        message.Append($"Request failed with status code {(int)response.StatusCode} ({response.ReasonPhrase}).");

        if (!string.IsNullOrWhiteSpace(content))
        {
            message.Append(' ').Append("Response content: ").Append(content);
        }

        throw new ApiException(message.ToString(), response, content);
    }
}
