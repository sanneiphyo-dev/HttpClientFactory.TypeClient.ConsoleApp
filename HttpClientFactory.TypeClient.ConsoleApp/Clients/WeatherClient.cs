using System.Globalization;
using System.Net.Http.Json;
using HttpClientFactory.TypeClient.ConsoleApp.Models;
using Microsoft.Extensions.Logging;

namespace HttpClientFactory.TypeClient.ConsoleApp.Clients;


public class WeatherClient(HttpClient httpClient, ILogger<WeatherClient> logger)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly ILogger<WeatherClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


    /// <param name="latitude">Latitude in decimal degrees (e.g., 16.8409).</param>
    /// <param name="longitude">Longitude in decimal degrees (e.g., 96.1735).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Strongly-typed <see cref="WeatherForecastResponse"
    public async Task<WeatherForecastResponse?> GetCurrentWeatherAsync(
        double latitude = 16.8409,
        double longitude = 96.1735,
        CancellationToken cancellationToken = default)
    {
        var latStr = latitude.ToString(CultureInfo.InvariantCulture);
        var lonStr = longitude.ToString(CultureInfo.InvariantCulture);
        var requestUri = $"v1/forecast?latitude={latStr}&longitude={lonStr}&current_weather=true";

        _logger.LogInformation("Requesting weather data for Lat: {Latitude}, Lon: {Longitude}", latStr, lonStr);
        try
        {
            var result = await _httpClient.GetFromJsonAsync<WeatherForecastResponse>(requestUri, cancellationToken);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to retrieve weather data from Open-Meteo API: {Message}", ex.Message);
            throw;
        }
    }
}
