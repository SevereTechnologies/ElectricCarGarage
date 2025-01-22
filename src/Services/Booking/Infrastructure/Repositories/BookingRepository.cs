using BookingGateway.Domain.Entities;

namespace BookingGateway.Infrastructure.Repositories;

public class BookingRepository(IDocumentSession session) : IBookingRepository
{
    public async Task<Booking?> GetBookingAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var booking = await session.LoadAsync<Booking>(customerId, cancellationToken);

        return booking;
    }

    public async Task<Booking> CreateBookingAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        session.Store(booking);
        await session.SaveChangesAsync(cancellationToken);
        return booking;
    }

    public async Task<bool> DeleteBookingAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        session.Delete<Booking>(customerId);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }
}
