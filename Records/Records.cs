using System.ComponentModel.DataAnnotations;

namespace TravelPlanningMCPAgent.Records;

public record PlanTripRequest : IValidatableObject
{
    [property: Required]
    [property: MinLength(3)]
    public string? Destination { get; set; }

    [property: Required]
    [property: DataType(DataType.Date)]
    public DateTime CheckIn { get; set; }

    [property: Required]
    [property: DataType(DataType.Date)]
    public DateTime CheckOut { get; set; }

    [property: Required]
    [property: Range(1, 20)]
    public int Guests { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => PlanTripRequestValidator.Validate(this);
}



public record PlanTripAdvancedRequest : PlanTripRequest
{
    [property: Required]
    [property: MinLength(3)]
    public string? Interests { get; set; }
}