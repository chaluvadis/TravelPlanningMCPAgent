using TravelPlanningMCPAgent.Agents;

namespace TravelPlanningMCPAgent.Client;

public class MCPClient(ILodgingAgent lodgingAgent, IItineraryAgent itineraryAgent, IWeatherAgent weatherAgent, IWebSearchAgent webSearchAgent) : IMCPClient
{
    private readonly ILodgingAgent _lodgingAgent = lodgingAgent;
    private readonly IItineraryAgent _itineraryAgent = itineraryAgent;
    private readonly IWeatherAgent _weatherAgent = weatherAgent;
    private readonly IWebSearchAgent _webSearchAgent = webSearchAgent;

    // Example method to coordinate agents and compile itinerary
    public async ValueTask<string> PlanTripAsync(string destination, DateTime checkIn, DateTime checkOut, int guests)
    {
        // 1. Get lodging options
        var lodging = await _lodgingAgent.FindLodgingAsync(destination, checkIn, checkOut, guests);
        // 2. Get itinerary suggestions
        var itinerary = await _itineraryAgent.PlanItineraryAsync(destination);
        // 3. Get weather forecast
        var weather = await _weatherAgent.GetWeatherAsync(destination, checkIn);
        // 4. Get travel tips
        var tips = await _webSearchAgent.GetTravelTipsAsync(destination);

        // 5. Compile results
        return $"Trip to {destination}:\nLodging: {lodging}\nItinerary: {itinerary}\nWeather: {weather}\nTips: {tips}";
    }
}
