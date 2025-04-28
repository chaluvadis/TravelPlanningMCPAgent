namespace TravelPlanningMCPAgent.Agents;
public interface IWebSearchAgent
{
    ValueTask<string> GetTravelTipsAsync(string location);
}