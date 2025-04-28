namespace TravelPlanningMCPAgent.Servers;
public interface IBookingServer
{
    IAsyncEnumerable<string> StreamAccommodationsAsync(string location, DateTime checkIn, DateTime checkOut, int guests);
    ValueTask<string> BookAccommodationAsync(string accommodationId, string userId);
}
