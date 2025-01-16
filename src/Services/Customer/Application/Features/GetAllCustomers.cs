namespace CustomerGateway.Application.Features;

public record GetAllCustomersQuery() : IQuery<GetAllCustomersResponse>;

public record GetAllCustomersResponse(IEnumerable<Customer> Customers);

internal class GetAllCustomersHandler(IDocumentSession session, ILogger<GetAllCustomersHandler> logger) : IQueryHandler<GetAllCustomersQuery, GetAllCustomersResponse>
{
    public async Task<GetAllCustomersResponse> Handle(GetAllCustomersQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Called GetAllCustomersHandler with {@query}", query);

        var customers = await session.Query<Customer>()
            .ToListAsync(cancellationToken);

        return new GetAllCustomersResponse(customers);
    }
}

public class GetAllCustomersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/customers", async (ISender sender) =>
        {
            var response = await sender.Send(new GetAllCustomersQuery());

            return Results.Ok(response);
        })
        .WithName("GetAllCustomers")
        .Produces<GetAllCustomersResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Customers");
    }
}
