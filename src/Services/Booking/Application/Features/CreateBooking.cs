using BookingGateway.Domain.Entities;
using BookingGateway.Infrastructure.Repositories;
using DiscountGateway.Presentation.gRPC.Protos;

namespace BookingGateway.Application.Features;

public record CreateBookingCommand(
    Booking booking) : ICommand<CreateBookingResponse>;

public record CreateBookingResponse(Guid CustomerId);

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.booking).NotNull().WithMessage("booking can not be null");
        RuleFor(x => x.booking.CustomerId).NotEmpty().WithMessage("Customer Id is required");
        RuleFor(x => x.booking.CustomerName).NotEmpty().WithMessage("Customer Name is required");
        RuleForEach(x => x.booking.Services).NotNull().WithMessage("At least 1 service is required.");
    }
}

internal class CreateBookingHandler(IBookingRepository repository, DiscountService.DiscountServiceClient discountGrpc) : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<CreateBookingResponse> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
    {
        // contact Discount.Grpc and get discount for product then calculate latest serfvice amount
        await ApplyDiscount(command.booking, cancellationToken);

        Booking booking = command.booking;

        //var booking = new Booking
        //{
        //    Id = Guid.NewGuid(),
        //    CustomerId = command.booking.CustomerId,
        //    CustomerName = command.booking.CustomerName,
        //    Services = command.booking.Services
        //};

        var result = await repository.CreateBookingAsync(booking);

        //return response
        return new CreateBookingResponse(result.CustomerId);
    }

    // apply the discount if any exist for each offer service in Booking
    private async Task ApplyDiscount(Booking booking, CancellationToken cancellationToken)
    {
        foreach (var service in booking.Services)
        {
            var offer = await discountGrpc.GetDiscountByIdAsync(new GetDiscountByIdRequest { Id = service.OfferId.ToString() }, cancellationToken: cancellationToken);
            service.Amount -= offer.Coupon.Amount;
        }
    }
}