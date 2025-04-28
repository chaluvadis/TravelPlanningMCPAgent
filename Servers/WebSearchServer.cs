using System.Text.Json;

namespace TravelPlanningMCPAgent.Servers;
public class WebSearchServer : IWebSearchServer
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public WebSearchServer(Microsoft.Extensions.Configuration.IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        var section = configuration.GetSection("BingSearch");
        _apiKey = section["ApiKey"] ?? "YOUR_BING_SEARCH_API_KEY_HERE";
        _baseUrl = section["BaseUrl"] ?? "https://api.bing.microsoft.com/v7.0/search";
    }

    public async ValueTask<IEnumerable<string>> SearchAsync(string query)
    {
        var url = $"{_baseUrl}?q={Uri.EscapeDataString(query)}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Ocp-Apim-Subscription-Key", _apiKey);
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var results = new List<string>();
        if (doc.RootElement.TryGetProperty("webPages", out var webPages) && webPages.TryGetProperty("value", out var value))
        {
            foreach (var item in value.EnumerateArray())
            {
                if (item.TryGetProperty("name", out var name))
                    results.Add(name.GetString() ?? "");
            }
        }
        return results;
    }
}
