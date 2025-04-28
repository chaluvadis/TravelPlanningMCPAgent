using System.ComponentModel.DataAnnotations;
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
                try
                {
                    var result = await mcpClient.PlanTripAsync(req.Destination!, req.CheckIn, req.CheckOut, req.Guests);
                    return Results.Ok(result);
                }
                catch (ValidationException vex)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]> {
                        { vex.ValidationAttribute?.GetType().Name ?? "ValidationError", new[] { vex.Message } }
                    });
                }
                catch (ArgumentException aex)
                {
                    return Results.BadRequest(aex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error planning trip: {ex.Message}");
                }
            });

        // Demo endpoint for planning a trip with user interests and cost estimation
        app.MapGet("/plantrip-advanced",
            async (IMCPClient mcpClient, [AsParameters] PlanTripAdvancedRequest req) =>
            {
                try
                {
                    var interestList = string.IsNullOrWhiteSpace(req.Interests) ? "tourist attractions" : req.Interests;
                    var lodging = await mcpClient.PlanTripAsync(req.Destination!, req.CheckIn, req.CheckOut, req.Guests);
                    var costEstimate = req.Guests * 100 * (req.CheckOut - req.CheckIn).Days;
                    var response = $"{lodging}\nInterests: {interestList}\nEstimated Cost: ${costEstimate}";
                    return Results.Ok(response);
                }
                catch (ValidationException vex)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]> {
                        { vex.ValidationAttribute?.GetType().Name ?? "ValidationError", new[] { vex.Message } }
                    });
                }
                catch (ArgumentException aex)
                {
                    return Results.BadRequest(aex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error planning advanced trip: {ex.Message}");
                }
            });
    }
}
