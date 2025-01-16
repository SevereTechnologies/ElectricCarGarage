namespace Catalog.API.Features;

public record CreateOfferingCommand(
    string Name,
    decimal Price,
    List<string> Categories,
    string Description) : ICommand<CreateOfferingResponse>;

public record CreateOfferingResponse(Guid Id);

public class CreateOfferingValidator : AbstractValidator<CreateOfferingCommand>
{
    public CreateOfferingValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class CreateOfferingHandler(IDocumentSession session) : ICommandHandler<CreateOfferingCommand, CreateOfferingResponse>
{
    public async Task<CreateOfferingResponse> Handle(CreateOfferingCommand command, CancellationToken cancellationToken)
    {
        var offering = new Offering
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Price = command.Price,
            Categories = command.Categories,
            Active = true,
            Description = command.Description
        };

        //track data
        session.Store(offering);

        //post to database
        await session.SaveChangesAsync(cancellationToken);

        //return response
        return new CreateOfferingResponse(offering.Id);
    }
}

public class CreateOfferingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/offerings",
            async (CreateOfferingCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Created($"/offerings/{response.Id}", response);
            })
        .WithName("CreateOffering")
        .Produces<CreateOfferingResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Offering");
    }
}