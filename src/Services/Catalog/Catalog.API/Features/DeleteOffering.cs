namespace Catalog.API.Features;

public record DeleteOfferingCommand(Guid Id) : ICommand<DeleteOfferingResponse>;

public record DeleteOfferingResponse(bool IsSuccess);

public class DeleteOfferingCommandValidator : AbstractValidator<DeleteOfferingCommand>
{
    public DeleteOfferingCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Offering ID is required");
    }
}

internal class DeleteOfferingHandler(IDocumentSession session) : ICommandHandler<DeleteOfferingCommand, DeleteOfferingResponse>
{
    public async Task<DeleteOfferingResponse> Handle(DeleteOfferingCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Offering>(command.Id);
        await session.SaveChangesAsync(cancellationToken);

        return new DeleteOfferingResponse(true);
    }
}

public class DeleteOfferingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/offerings/{id}", async (Guid id, ISender sender) =>
        {
            var response = await sender.Send(new DeleteOfferingCommand(id));

            return Results.Ok(response);
        })
        .WithName("DeleteOffering")
        .Produces<DeleteOfferingResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Offering");
    }
}