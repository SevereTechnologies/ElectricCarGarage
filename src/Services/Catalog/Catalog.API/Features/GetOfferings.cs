namespace Catalog.API.Features;

public record GetOfferingsQuery() : IQuery<GetOfferingsResponse>;

public record GetOfferingsResponse(IEnumerable<Offering> Offerings);

internal class GetOfferingsHandler(IDocumentSession session, ILogger<GetOfferingsHandler> logger) : IQueryHandler<GetOfferingsQuery, GetOfferingsResponse>
{
    public async Task<GetOfferingsResponse> Handle(GetOfferingsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Called GetOfferingsHandler with {@query}", query);

        var offerings = await session.Query<Offering>()
            .ToListAsync(cancellationToken);

        return new GetOfferingsResponse(offerings);
    }
}

public class GetOfferingsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/offerings", async (ISender sender) =>
        {
            var response = await sender.Send(new GetOfferingsQuery());

            return Results.Ok(response);
        })
        .WithName("GetOfferings")
        .Produces<GetOfferingsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Offerings");
    }
}