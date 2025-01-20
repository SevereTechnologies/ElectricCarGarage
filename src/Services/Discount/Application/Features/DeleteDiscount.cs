namespace DiscountGateway.Application.Features;

public record DeleteDiscountCommand(Guid Id) : ICommand<DeleteDiscountResponse>;

public record DeleteDiscountResponse(bool IsSuccess);

public class DeleteDiscountCommandValidator : AbstractValidator<DeleteDiscountCommand>
{
    public DeleteDiscountCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Discount ID is required");
    }
}

internal class DeleteDiscountHandler(IDocumentSession session) : ICommandHandler<DeleteDiscountCommand, DeleteDiscountResponse>
{
    public async Task<DeleteDiscountResponse> Handle(DeleteDiscountCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Coupon>(command.Id);
        await session.SaveChangesAsync(cancellationToken);

        return new DeleteDiscountResponse(true);
    }
}

public class DeleteDiscountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/discounts/{id}", async (Guid id, ISender sender) =>
        {
            var response = await sender.Send(new DeleteDiscountCommand(id));

            return Results.Ok(response);
        })
        .WithName("DeleteDiscount")
        .Produces<DeleteDiscountResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Discount")
        .WithDescription("Delete Discount");
    }
}