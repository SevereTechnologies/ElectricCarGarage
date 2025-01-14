
namespace Catalog.API.Models;

public class Offering
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required List<string> Categories { get; set; }
    public bool Active { get; set; } = true;
    public string Description { get; set; } = default!;
}