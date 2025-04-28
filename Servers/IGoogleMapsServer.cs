namespace TravelPlanningMCPAgent.Servers;
public interface IGoogleMapsServer
{
    ValueTask<IEnumerable<string>> SearchPlacesAsync(string location, string query);
    ValueTask<string> GetDirectionsAsync(string origin, string destination, string mode);
}