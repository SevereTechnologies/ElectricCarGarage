namespace CustomerGateway.Application.Features;

public record CreateCustomerCommand(
    string Name,
    string Phone,
    string Email,
    string Address,
    string City,
    string State,
    string Zip,
    string Note) : ICommand<CreateCustomerResponse>;

public record CreateCustomerResponse(Guid Id);

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
    }
}

internal class CreateCustomerHandler(IDocumentSession session) : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Phone = command.Phone,
            Email = command.Email,
            Address = command.Address,
            City = command.City,
            State = command.State,
            Zip = command.Zip,
            Note = command.Note,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        //track data
        session.Store(customer);

        //post to database
        await session.SaveChangesAsync(cancellationToken);

        //return response
        return new CreateCustomerResponse(customer.Id);
    }
}

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