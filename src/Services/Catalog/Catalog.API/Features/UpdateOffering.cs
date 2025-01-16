namespace Catalog.API.Features;

public record UpdateOfferingCommand(
    Guid Id,
    string Name,
    decimal Price,
    List<string> Categories,
    bool Active,
    string Description) : ICommand<UpdateOfferingResponse>;

public record UpdateOfferingResponse(bool IsSuccess, string Message);

public class UpdateOfferingValidator : AbstractValidator<UpdateOfferingCommand>
{
    public UpdateOfferingValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Offering Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateOfferingHandler(IDocumentSession session) : ICommandHandler<UpdateOfferingCommand, UpdateOfferingResponse>
{
    public async Task<UpdateOfferingResponse> Handle(UpdateOfferingCommand command, CancellationToken cancellationToken)
    {
        var offering = await session.LoadAsync<Offering>(command.Id, cancellationToken);

        if (offering is null)
        {
            return new UpdateOfferingResponse(false, "Not Found");
        }

        // update offering info
        offering.Name = command.Name;
        offering.Price = command.Price;
        offering.Categories = command.Categories;
        offering.Active = command.Active;
        offering.Description = command.Description;

        // track data
        session.Update(offering);

        // post to db
        await session.SaveChangesAsync(cancellationToken);

        // return result
        return new UpdateOfferingResponse(true, "Updated!");
    }
}

public class UpdateOfferingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/offerings",
            async (UpdateOfferingCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Ok(response);
            })
            .WithName("UpdateOffering WithName")
            .Produces<UpdateOfferingResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Offering WithSummary")
            .WithDescription("Update Offering WithDescription");
    }
}