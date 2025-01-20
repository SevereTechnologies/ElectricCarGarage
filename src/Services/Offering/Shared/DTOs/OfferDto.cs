namespace OfferingGateway.Shared.DTOs;

public class OfferDto
{
    public required string Id { get; set; }
    public required string ServiceName { get; set; } = default!;
    public required List<string> Category { get; set; } = new();
    public string Description { get; set; } = default!;
    public required decimal Price { get; set; }
    public bool Active { get; set; } = true;
}
