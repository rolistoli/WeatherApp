using System.Text.Json.Serialization;

namespace WeatherApp.DTOs;
public sealed class NominatimReverseDto
{
    [JsonPropertyName("place_id")]
    public long PlaceId { get; init; }

    [JsonPropertyName("licence")]
    public string Licence { get; init; } = string.Empty;

    [JsonPropertyName("osm_type")]
    public string OsmType { get; init; } = string.Empty;

    [JsonPropertyName("osm_id")]
    public long OsmId { get; init; }

    [JsonPropertyName("lat")]
    public string Lat { get; init; } = string.Empty;

    [JsonPropertyName("lon")]
    public string Lon { get; init; } = string.Empty;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; init; } = string.Empty;

    [JsonPropertyName("address")]
    public NominatimAddressDto Address { get; init; } = new();
}

public sealed class NominatimAddressDto
{
    [JsonPropertyName("city")]
    public string? City { get; init; }

    [JsonPropertyName("town")]
    public string? Town { get; init; }

    [JsonPropertyName("village")]
    public string? Village { get; init; }

    [JsonPropertyName("county")]
    public string? County { get; init; }

    [JsonPropertyName("state")]
    public string? State { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }
}
