using System.Text.Json.Serialization;

namespace WeatherApp.DTOs;

public sealed class GeocodingSearchResponseDto
{
    [JsonPropertyName("results")]
    public List<GeocodingSearchResultDto> Results { get; init; } = [];

    [JsonPropertyName("generationtime_ms")]
    public double GenerationTimeMs { get; init; }
}

public abstract class GeocodingSearchResultDto
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("latitude")]
    public double Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; init; }

    [JsonPropertyName("elevation")]
    public double Elevation { get; init; }

    [JsonPropertyName("feature_code")]
    public string FeatureCode { get; init; } = string.Empty;

    [JsonPropertyName("country_code")]
    public string CountryCode { get; init; } = string.Empty;

    [JsonPropertyName("admin1_id")]
    public long? Admin1Id { get; init; }

    [JsonPropertyName("admin2_id")]
    public long? Admin2Id { get; init; }

    [JsonPropertyName("admin3_id")]
    public long? Admin3Id { get; init; }

    [JsonPropertyName("admin4_id")]
    public long? Admin4Id { get; init; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; init; } = string.Empty;

    [JsonPropertyName("population")]
    public long? Population { get; init; }

    [JsonPropertyName("country_id")]
    public long? CountryId { get; init; }

    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;

    [JsonPropertyName("admin1")]
    public string Admin1 { get; init; } = string.Empty;

    [JsonPropertyName("admin2")]
    public string Admin2 { get; init; } = string.Empty;

    [JsonPropertyName("admin3")]
    public string Admin3 { get; init; } = string.Empty;

    [JsonPropertyName("admin4")]
    public string Admin4 { get; init; } = string.Empty;
}
