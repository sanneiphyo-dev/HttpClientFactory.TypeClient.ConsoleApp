using System.Text.Json.Serialization;

namespace HttpClientFactory.TypeClient.ConsoleApp.Models;


/// Root response model returned by the Open-Meteo forecast API.

public sealed record WeatherForecastResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("timezone")] string? Timezone,
    [property: JsonPropertyName("elevation")] double? Elevation,
    [property: JsonPropertyName("current_weather_units")] CurrentWeatherUnits? CurrentWeatherUnits,
    [property: JsonPropertyName("current_weather")] CurrentWeather? CurrentWeather
);


/// Units description for current weather fields.

public sealed record CurrentWeatherUnits(
    [property: JsonPropertyName("temperature")] string? TemperatureUnit,
    [property: JsonPropertyName("windspeed")] string? WindSpeedUnit
);


/// Real-time weather observation metrics.

public sealed record CurrentWeather(
    [property: JsonPropertyName("temperature")] double Temperature,
    [property: JsonPropertyName("windspeed")] double WindSpeed,
    [property: JsonPropertyName("winddirection")] double WindDirection,
    [property: JsonPropertyName("weathercode")] int WeatherCode,
    [property: JsonPropertyName("is_day")] int IsDay,
    [property: JsonPropertyName("time")] string? Time
);
