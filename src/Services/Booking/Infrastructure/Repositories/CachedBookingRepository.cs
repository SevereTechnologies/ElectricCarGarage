using BookingGateway.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BookingGateway.Infrastructure.Repositories;

public class CachedBookingRepository(IBookingRepository repository, IDistributedCache cache) : IBookingRepository
{
    public async Task<Booking?> GetBookingAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cachedBooking = await cache.GetStringAsync(customerId.ToString(), cancellationToken);

        if (!string.IsNullOrEmpty(cachedBooking))
        {
            return JsonSerializer.Deserialize<Booking>(cachedBooking)!;
        }

        var booking = await repository.GetBookingAsync(customerId, cancellationToken);

        await cache.SetStringAsync(customerId.ToString(), JsonSerializer.Serialize(booking), cancellationToken);

        return booking;
    }

    public async Task<Booking> CreateBookingAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await repository.CreateBookingAsync(booking, cancellationToken);

        await cache.SetStringAsync(booking.CustomerId.ToString(), JsonSerializer.Serialize(booking), cancellationToken);

        return booking;
    }

    public async Task<bool> DeleteBookingAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBookingAsync(customerId, cancellationToken);

        await cache.RemoveAsync(customerId.ToString(), cancellationToken);

        return true;
    }
}