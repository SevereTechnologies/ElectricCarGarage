using OfferingGateway.Domain.Entities;

namespace OfferGateway.Application.Features;

public record UpdateOfferCommand(
    Guid Id,
    string ServiceName,
    List<string> Category,
    string Description,
    decimal Price,
    bool Active) : ICommand<UpdateOfferResponse>;

public record UpdateOfferResponse(bool IsSuccess, string Message);

public class UpdateOfferValidator : AbstractValidator<UpdateOfferCommand>
{
    public UpdateOfferValidator()
    {
        RuleFor(x => x.ServiceName).NotEmpty().WithMessage("ServiceName is required");
        RuleForEach(x => x.Category).NotEmpty().MaximumLength(50).WithMessage("Category is required and cannot be less than 50 characters.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is required and must be greater than 0.");
    }
}

internal class UpdateOfferHandler(IDocumentSession session) : ICommandHandler<UpdateOfferCommand, UpdateOfferResponse>
{
    public async Task<UpdateOfferResponse> Handle(UpdateOfferCommand command, CancellationToken cancellationToken)
    {
        var offer = await session.LoadAsync<Offer>(command.Id, cancellationToken);

        if (offer is null)
        {
            return new UpdateOfferResponse(false, "Not Found");
        }

        // update offer info
        offer.Id = command.Id;
        offer.ServiceName = command.ServiceName;
        offer.Category = command.Category;
        offer.Description = command.Description;
        offer.Price = command.Price;
        offer.Active = command.Active;

        // track data
        session.Update(offer);

        // post to db
        await session.SaveChangesAsync(cancellationToken);

        // return result
        return new UpdateOfferResponse(true, "Updated!");
    }
}
