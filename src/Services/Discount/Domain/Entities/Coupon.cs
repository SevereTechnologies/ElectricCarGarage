namespace DiscountGateway.Domain.Entities;

public record Coupon
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;
}
