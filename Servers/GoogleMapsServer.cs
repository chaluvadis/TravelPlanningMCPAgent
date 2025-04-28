using System.Text.Json;

namespace TravelPlanningMCPAgent.Servers;

public class GoogleMapsServer : IGoogleMapsServer
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrlPlaces;
    private readonly string _baseUrlDirections;

    public GoogleMapsServer(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        var section = configuration.GetSection("GoogleMaps");
        _apiKey = section["ApiKey"] ?? "YOUR_GOOGLE_MAPS_API_KEY_HERE";
        _baseUrlPlaces = section["PlacesUrl"] ?? "https://maps.googleapis.com/maps/api/place/textsearch/json";
        _baseUrlDirections = section["DirectionsUrl"] ?? "https://maps.googleapis.com/maps/api/directions/json";
    }

    public async ValueTask<IEnumerable<string>> SearchPlacesAsync(string location, string query)
    {
        var url = $"{_baseUrlPlaces}?query={Uri.EscapeDataString(query + " in " + location)}&key={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var results = new List<string>();
        if (doc.RootElement.TryGetProperty("results", out var places))
        {
            foreach (var place in places.EnumerateArray())
            {
                if (place.TryGetProperty("name", out var name))
                    results.Add(name.GetString() ?? "");
            }
        }
        return results;
    }

    public async ValueTask<string> GetDirectionsAsync(string origin, string destination, string mode)
    {
        var url = $"{_baseUrlDirections}?origin={Uri.EscapeDataString(origin)}&destination={Uri.EscapeDataString(destination)}&mode={Uri.EscapeDataString(mode)}&key={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("routes", out var routes) && routes.GetArrayLength() > 0)
        {
            var summary = routes[0].GetProperty("summary").GetString();
            return summary ?? "Route found";
        }
        return "No route found";
    }
}