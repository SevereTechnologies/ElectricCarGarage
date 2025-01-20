namespace OfferingGateway.Domain.Entities;

public class Offer
{
    public required Guid Id { get; set; }
    public required string ServiceName { get; set; } = default!;
    public required List<string> Category { get; set; } = new();
    public string Description { get; set; } = default!;
    public required decimal Price { get; set; }
    public bool Active { get; set; } = true;
}
