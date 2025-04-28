namespace TravelPlanningMCPAgent.Agents;
public interface IItineraryAgent
{
    ValueTask<string> PlanItineraryAsync(string location);
    ValueTask<string> PlanItineraryAsync(string location, string interests);
}