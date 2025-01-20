namespace DiscountGateway.Application.Features;

public record CreateDiscountCommand(
    string Name,
    string Description,
    int Amount) : ICommand<CreateDiscountResponse>;

public record CreateDiscountResponse(Guid Id);

public class CreateDiscountValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount is required");
    }
}

internal class CreateDiscountHandler(IDocumentSession session) : ICommandHandler<CreateDiscountCommand, CreateDiscountResponse>
{
    public async Task<CreateDiscountResponse> Handle(CreateDiscountCommand command, CancellationToken cancellationToken)
    {
        var coupon = new Coupon
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Amount = command.Amount,
            ExpirationDate = DateTime.UtcNow
        };

        //track data
        session.Store(coupon);

        //post to database
        await session.SaveChangesAsync(cancellationToken);

        //return response
        return new CreateDiscountResponse(coupon.Id);
    }
}