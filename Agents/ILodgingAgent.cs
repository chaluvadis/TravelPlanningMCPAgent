namespace TravelPlanningMCPAgent.Agents;
public interface ILodgingAgent
{
    ValueTask<string> FindLodgingAsync(string location, DateTime checkIn, DateTime checkOut, int guests);
}