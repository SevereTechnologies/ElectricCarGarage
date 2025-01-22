using BookingGateway.Domain.Entities;

namespace BookingGateway.Application.Features;

public record GetBookingQuery(Guid customerId) : IQuery<GetBookingResponse>;

public record GetBookingResponse(Booking Offers);

internal class GetBookingQueryHandler(IDocumentSession session, ILogger<GetBookingQueryHandler> logger) : IQueryHandler<GetBookingQuery, GetBookingResponse>
{
    public async Task<GetBookingResponse> Handle(GetBookingQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Called GetBookingQueryHandler with {@query}", query);

        var booking = await session.Query<Booking>().FirstOrDefaultAsync(cancellationToken);

        return new GetBookingResponse(booking);
    }
}
