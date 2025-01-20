using OfferingGateway.Domain.Entities;

namespace OfferingGateway.Application.Features;

public record CreateOfferCommand(
    string ServiceName,
    List<string> Category,
    string Description,
    decimal Price) : ICommand<CreateOfferResponse>;

public record CreateOfferResponse(Guid Id);

public class CreateOfferValidator : AbstractValidator<CreateOfferCommand>
{
    public CreateOfferValidator()
    {
        RuleFor(x => x.ServiceName).NotEmpty().WithMessage("ServiceName is required");
        RuleForEach(x => x.Category).NotEmpty().MaximumLength(50).WithMessage("Category is required and cannot be less than 50 characters.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is required and must be greater than 0.");
    }
}

internal class CreateOfferHandler(IDocumentSession session) : ICommandHandler<CreateOfferCommand, CreateOfferResponse>
{
    public async Task<CreateOfferResponse> Handle(CreateOfferCommand command, CancellationToken cancellationToken)
    {
        var offer = new Offer
        {
            Id = Guid.NewGuid(),
            ServiceName = command.ServiceName,
            Category = command.Category,
            Description = command.Description,
            Price = command.Price,
            Active = true
        };

        //track data
        session.Store(offer);

        //post to database
        await session.SaveChangesAsync(cancellationToken);

        //return response
        return new CreateOfferResponse(offer.Id);
    }
}
