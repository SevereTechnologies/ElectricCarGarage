namespace CustomerGateway.Application.Features;

public record GetCustomerByIdQuery(Guid Id) : IQuery<GetCustomerByIdResponse>;

public record GetCustomerByIdResponse(Customer? customer);

internal class GetCustomerByIdHandler(IDocumentSession session) : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdResponse>
{
    public async Task<GetCustomerByIdResponse> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await session.LoadAsync<Customer>(query.Id, cancellationToken);

        return new GetCustomerByIdResponse(customer);
    }
}

public class GetCustomerByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/customers/{id}", async (Guid id, ISender sender) =>
        {
            var respnse = await sender.Send(new GetCustomerByIdQuery(id));

            return Results.Ok(respnse);
        })
        .WithName("GetCustomerById")
        .Produces<GetCustomerByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Customer By Id");
    }
}