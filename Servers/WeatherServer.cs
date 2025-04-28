using System.Text.Json;

namespace TravelPlanningMCPAgent.Servers;
public class WeatherServer : IWeatherServer
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public WeatherServer(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        var section = configuration.GetSection("OpenWeather");
        _apiKey = section["ApiKey"] ?? "YOUR_OPENWEATHER_API_KEY_HERE";
        _baseUrl = section["BaseUrl"] ?? "https://api.openweathermap.org/data/2.5/forecast";
    }

    public async ValueTask<string> GetWeatherForecastAsync(string location, DateTime date)
    {
        var url = $"{_baseUrl}?q={Uri.EscapeDataString(location)}&appid={_apiKey}&units=metric";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("list", out var forecasts))
        {
            foreach (var forecast in forecasts.EnumerateArray())
            {
                if (forecast.TryGetProperty("dt_txt", out var dtTxt))
                {
                    if (DateTime.TryParse(dtTxt.GetString(), out var forecastDate))
                    {
                        if (forecastDate.Date == date.Date)
                        {
                            var main = forecast.GetProperty("main");
                            var temp = main.GetProperty("temp").GetDouble();
                            var weather = forecast.GetProperty("weather")[0].GetProperty("description").GetString();
                            return $"{weather}, {temp}°C in {location} on {date:MMMM dd}";
                        }
                    }
                }
            }
        }
        return $"No forecast found for {location} on {date:MMMM dd}";
    }
}
