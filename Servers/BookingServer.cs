namespace TravelPlanningMCPAgent.Servers;
public class BookingServer : IBookingServer
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public BookingServer(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        var section = configuration.GetSection("Booking");
        _apiKey = section["ApiKey"] ?? "YOUR_BOOKING_API_KEY_HERE";
        _baseUrl = section["BaseUrl"] ?? "https://distribution-xml.booking.com/json/";
    }

    public async ValueTask<string> BookAccommodationAsync(string accommodationId, string userId)
    {
        // Read booking endpoint from config
        var bookingUrl = _baseUrl.TrimEnd('/') + "/bookings.createBooking";
        var bookingSecret = Environment.GetEnvironmentVariable("BOOKING_COM_SECRET") ?? "YOUR_BOOKING_COM_SECRET";
        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_apiKey}:{bookingSecret}");
        var request = new HttpRequestMessage(HttpMethod.Post, bookingUrl);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        request.Content = new StringContent($"{{\"hotel_id\":\"{accommodationId}\",\"user_id\":\"{userId}\"}}", System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Booking failed: {response.StatusCode} - {error}");
        }
        return await response.Content.ReadAsStringAsync();
    }

    public async IAsyncEnumerable<string> StreamAccommodationsAsync(string location, DateTime checkIn, DateTime checkOut, int guests)
    {
        // Read hotels endpoint from config
        var hotelsUrl = _baseUrl.TrimEnd('/') + $"/bookings.getHotels?city_ids={Uri.EscapeDataString(location)}&checkin={checkIn:yyyy-MM-dd}&checkout={checkOut:yyyy-MM-dd}&room1=A,{guests}";
        var bookingSecret = Environment.GetEnvironmentVariable("BOOKING_COM_SECRET") ?? "YOUR_BOOKING_COM_SECRET";
        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_apiKey}:{bookingSecret}");
        var request = new HttpRequestMessage(HttpMethod.Get, hotelsUrl);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = System.Text.Json.JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("result", out var hotels))
        {
            foreach (var hotel in hotels.EnumerateArray())
            {
                var name = hotel.GetProperty("hotel_name").GetString();
                var address = hotel.GetProperty("address").GetString();
                yield return $"{name} - {address}";
            }
        }
    }
}
