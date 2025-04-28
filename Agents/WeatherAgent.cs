namespace TravelPlanningMCPAgent.Agents;

public class WeatherAgent(IWeatherServer weatherServer) : IWeatherAgent
{
    private readonly IWeatherServer _weatherServer = weatherServer;

    public async ValueTask<string> GetWeatherAsync(string location, DateTime date)
        => await _weatherServer.GetWeatherForecastAsync(location, date);
}