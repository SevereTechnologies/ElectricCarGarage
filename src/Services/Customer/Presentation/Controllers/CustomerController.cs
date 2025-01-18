namespace CustomerGateway.Presentation.Controllers;

public class CreateCustomerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/customers",
            async (CreateCustomerCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Created($"/customers/{response.Id}", response);
            })
        .WithName("CreateCustomer")
        .Produces<CreateCustomerResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Customer");
    }
}

public class UpdateCustomerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/customers",
            async (UpdateCustomerCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Ok(response);
            })
            .WithName("UpdateCustomer WithName")
            .Produces<UpdateCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Customer WithSummary")
            .WithDescription("Update Customer WithDescription");
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
