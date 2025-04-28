namespace TravelPlanningMCPAgent.Agents;

public class WebSearchAgent(IWebSearchServer webSearchServer) : IWebSearchAgent
{
    private readonly IWebSearchServer _webSearchServer = webSearchServer;

    public async ValueTask<string> GetTravelTipsAsync(string location)
    {
        var results = await _webSearchServer.SearchAsync($"travel tips for {location}");
        return string.Join(", ", results);
    }
}
