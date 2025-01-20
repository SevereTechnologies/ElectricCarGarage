using OfferingGateway.Domain.Entities;

namespace OfferGateway.Application.Features;

public record GetOfferByIdQuery(Guid Id) : IQuery<GetOfferByIdResponse>;

public record GetOfferByIdResponse(Offer? offer);

internal class GetOfferByIdHandler(IDocumentSession session) : IQueryHandler<GetOfferByIdQuery, GetOfferByIdResponse>
{
    public async Task<GetOfferByIdResponse> Handle(GetOfferByIdQuery query, CancellationToken cancellationToken)
    {
        var offer = await session.LoadAsync<Offer>(query.Id, cancellationToken);

        return new GetOfferByIdResponse(offer);
    }
}
