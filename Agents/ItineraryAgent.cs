using TravelPlanningMCPAgent.Servers;
namespace TravelPlanningMCPAgent.Agents;

public class ItineraryAgent(IGoogleMapsServer mapsServer) : IItineraryAgent
{
    private readonly IGoogleMapsServer _mapsServer = mapsServer;

    public async ValueTask<string> PlanItineraryAsync(string location)
    {
        var results = await _mapsServer.SearchPlacesAsync(location, "tourist attractions");
        return string.Join(", ", results);
    }
    // Overload to support user interests
    public async ValueTask<string> PlanItineraryAsync(string location, string interests)
    {
        var results = await _mapsServer.SearchPlacesAsync(location, interests);
        return string.Join(", ", results);
    }
}