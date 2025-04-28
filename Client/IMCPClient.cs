namespace TravelPlanningMCPAgent.Client;
public interface IMCPClient
{
    ValueTask<string> PlanTripAsync(string destination, DateTime checkIn, DateTime checkOut, int guests);
}