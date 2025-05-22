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
        var lodgingTask = _lodgingAgent.FindLodgingAsync(destination, checkIn, checkOut, guests).AsTask();
        var itineraryTask = _itineraryAgent.PlanItineraryAsync(destination).AsTask();
        var weatherTask = _weatherAgent.GetWeatherAsync(destination, checkIn).AsTask();
        var tipsTask = _webSearchAgent.GetTravelTipsAsync(destination).AsTask();

        await Task.WhenAll(lodgingTask, itineraryTask, weatherTask, tipsTask);

        var lodging = await lodgingTask;
        var itinerary = await itineraryTask;
        var weather = await weatherTask;
        var tips = await tipsTask;

        return $"Trip to {destination}:\nLodging: {lodging}\nItinerary: {itinerary}\nWeather: {weather}\nTips: {tips}";
    }
}
