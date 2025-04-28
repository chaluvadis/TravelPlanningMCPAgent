using TravelPlanningMCPAgent.Servers;
public interface IWeatherServer
{
    ValueTask<string> GetWeatherForecastAsync(string location, DateTime date);
}