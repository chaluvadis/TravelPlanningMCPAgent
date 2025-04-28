using TravelPlanningMCPAgent.Servers;
namespace TravelPlanningMCPAgent.Agents;

public class LodgingAgent(IBookingServer bookingServer) : ILodgingAgent
{
    private readonly IBookingServer _bookingServer = bookingServer;

    public async ValueTask<string> FindLodgingAsync(string location, DateTime checkIn, DateTime checkOut, int guests)
    {
        await foreach (var result in _bookingServer.StreamAccommodationsAsync(location, checkIn, checkOut, guests))
        {
            return result ?? "No lodging found";
        }
        return "No lodging found";
    }
}