using TravelPlanningMCPAgent.Client;
using TravelPlanningMCPAgent.Records;

namespace TravelPlanningMCPAgent.Routes;

public static class TripRoutes
{
    public static void MapTripRoutes(this WebApplication app)
    {
        // Demo endpoint for planning a trip
        app.MapGet("/plantrip",
            async (IMCPClient mcpClient, [AsParameters] PlanTripRequest req) =>
            {
                // Validation is handled by DataAnnotations and the framework in .NET 10+
                var result = await mcpClient.PlanTripAsync(req.Destination!, req.CheckIn, req.CheckOut, req.Guests);
                return Results.Ok(result);
            });

        // Demo endpoint for planning a trip with user interests and cost estimation
        app.MapGet("/plantrip-advanced",
            async (IMCPClient mcpClient, [AsParameters] PlanTripAdvancedRequest req) =>
            {
                var interestList = string.IsNullOrWhiteSpace(req.Interests) ? "tourist attractions" : req.Interests;
                var lodging = await mcpClient.PlanTripAsync(req.Destination!, req.CheckIn, req.CheckOut, req.Guests);
                var costEstimate = req.Guests * 100 * (req.CheckOut - req.CheckIn).Days;
                var response = $"{lodging}\nInterests: {interestList}\nEstimated Cost: ${costEstimate}";
                return Results.Ok(response);
            });
    }
}
