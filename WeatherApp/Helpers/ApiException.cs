namespace WeatherApp.Helpers;

public sealed class ApiException : Exception
{
    public HttpResponseMessage? Response { get; }
    public string? ResponseContent { get; }

    public ApiException(string message, HttpResponseMessage? response = null, string? responseContent = null)
        : base(message)
    {
        Response = response;
        ResponseContent = responseContent;
    }

    public ApiException(string message, Exception innerException, HttpResponseMessage? response = null, string? responseContent = null)
        : base(message, innerException)
    {
        Response = response;
        ResponseContent = responseContent;
    }
}
