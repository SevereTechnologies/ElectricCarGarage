namespace DiscountGateway.Shared.DTOs;

public record CouponDto
{
    public string Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
    public string ExpirationDate { get; set; } = default!;
}
