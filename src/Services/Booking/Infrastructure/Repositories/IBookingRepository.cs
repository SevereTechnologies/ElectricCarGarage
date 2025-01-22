using BookingGateway.Domain.Entities;

namespace BookingGateway.Infrastructure.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetBookingAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Booking> CreateBookingAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<bool> DeleteBookingAsync(Guid customerId, CancellationToken cancellationToken = default);
}
