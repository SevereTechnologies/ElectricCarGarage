namespace BookingGateway.Domain.Entities;

//public class BookingService
//{
//    public decimal Amount { get; set; } = default!;
//    public Guid OfferId { get; set; } = default!;
//    public string OfferName { get; set; } = default!;
//}

public class BookingService
{
    public decimal Original { get; set; } = default!;
    public decimal Discount { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public Guid OfferId { get; set; } = default!;
    public string OfferName { get; set; } = default!;
}