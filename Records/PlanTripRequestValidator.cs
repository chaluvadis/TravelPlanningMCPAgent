using System.ComponentModel.DataAnnotations;

namespace TravelPlanningMCPAgent.Records;

public static class PlanTripRequestValidator
{
    public static IEnumerable<ValidationResult> Validate(PlanTripRequest req)
    {
        if (req.CheckIn >= req.CheckOut)
        {
            yield return new ValidationResult(
                "Check-in date must be before check-out date.",
                [nameof(req.CheckIn), nameof(req.CheckOut)]);
        }
        if (req.CheckIn < DateTime.Today)
        {
            yield return new ValidationResult(
                "Check-in date cannot be in the past.",
                [nameof(req.CheckIn)]);
        }
    }
}
