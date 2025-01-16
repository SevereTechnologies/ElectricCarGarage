namespace Catalog.API.Features;

public record GetOfferingByIdQuery(Guid Id) : IQuery<GetOfferingByIdResponse>;

public record GetOfferingByIdResponse(Offering? offering);

internal class GetOfferingByIdHandler(IDocumentSession session) : IQueryHandler<GetOfferingByIdQuery, GetOfferingByIdResponse>
{
    public async Task<GetOfferingByIdResponse> Handle(GetOfferingByIdQuery query, CancellationToken cancellationToken)
    {
        var offering = await session.LoadAsync<Offering>(query.Id, cancellationToken);

        return new GetOfferingByIdResponse(offering);
    }
}

public class GetOfferingByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/offerings/{id}", async (Guid id, ISender sender) =>
        {
            var respnse = await sender.Send(new GetOfferingByIdQuery(id));

            return Results.Ok(respnse);
        })
        .WithName("GetOfferingById")
        .Produces<GetOfferingByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Offering By Id");
    }
}