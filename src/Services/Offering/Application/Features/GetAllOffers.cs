using OfferingGateway.Domain.Entities;

namespace OfferGateway.Application.Features;

public record GetAllOffersQuery() : IQuery<GetAllOffersResponse>;

public record GetAllOffersResponse(IEnumerable<Offer> Offers);

internal class GetAllOffersHandler(IDocumentSession session, ILogger<GetAllOffersHandler> logger) : IQueryHandler<GetAllOffersQuery, GetAllOffersResponse>
{
    public async Task<GetAllOffersResponse> Handle(GetAllOffersQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Called GetAllOffersHandler with {@query}", query);

        var offers = await session.Query<Offer>()
            .ToListAsync(cancellationToken);

        return new GetAllOffersResponse(offers);
    }
}


