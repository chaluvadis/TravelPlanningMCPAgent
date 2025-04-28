namespace TravelPlanningMCPAgent.Agents;
public interface IWeatherAgent
{
    ValueTask<string> GetWeatherAsync(string location, DateTime date);
}