namespace BookingGateway.Domain.Entities;

public class Booking
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public List<BookingService> Services { get; set; } = new();
    public decimal TotalAmount => Services.Sum(x => x.Amount);
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public Booking(Guid customerId, string customerName)
    {
        CustomerId = customerId;
        CustomerName = customerName;
    }

    public Booking()
    {

    }
}
