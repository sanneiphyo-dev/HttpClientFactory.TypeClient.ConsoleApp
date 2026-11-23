using HttpClientFactory.TypeClient.ConsoleApp.Clients;
using HttpClientFactory.TypeClient.ConsoleApp.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = false;
    options.SingleLine = true;
    options.TimestampFormat = "[HH:mm:ss] ";
});

builder.Services.AddTransient<SimpleTraceHandler>();

builder.Services.AddHttpClient<WeatherClient>(client =>
{
    // Base address for Open-Meteo public REST API
    client.BaseAddress = new Uri("https://api.open-meteo.com/");
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<SimpleTraceHandler>(); 


// 4. Build the Host container
using var host = builder.Build();


var weatherClient = host.Services.GetRequiredService<WeatherClient>();

try
{
    // 6. Execute sample API call (default coordinates: Yangon, Myanmar -> Lat: 16.8409, Lon: 96.1735)
    var forecast = await weatherClient.GetCurrentWeatherAsync(16.8409, 96.1735);

    // 7. Print a clear, formatted summary to the console
    Console.WriteLine();
    Console.WriteLine("------------------- Weather Report Summary -------------------");
    if (forecast is not null && forecast.CurrentWeather is not null)
    {
        var tempUnit = forecast.CurrentWeatherUnits?.TemperatureUnit ?? "°C";
        var windUnit = forecast.CurrentWeatherUnits?.WindSpeedUnit ?? "km/h";

        Console.WriteLine($" Location       : Latitude {forecast.Latitude:F4}, Longitude {forecast.Longitude:F4}");
        Console.WriteLine($" Timezone       : {forecast.Timezone ?? "N/A"} (Elevation: {forecast.Elevation}m)");
        Console.WriteLine($" Temperature    : {forecast.CurrentWeather.Temperature} {tempUnit}");
        Console.WriteLine($" Wind Speed     : {forecast.CurrentWeather.WindSpeed} {windUnit}");
        Console.WriteLine($" Wind Direction : {forecast.CurrentWeather.WindDirection}°");
        Console.WriteLine($" Weather Code   : {forecast.CurrentWeather.WeatherCode}");
        Console.WriteLine($" Observation    : {forecast.CurrentWeather.Time}");
    }
  
    Console.WriteLine("Test completed successfully!");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"[ERROR] An error occurred while testing WeatherClient: {ex.Message}");
    Console.ResetColor();
}
