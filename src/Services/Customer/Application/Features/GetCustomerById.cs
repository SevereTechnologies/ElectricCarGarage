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
