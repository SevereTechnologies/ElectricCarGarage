using BookingGateway.Domain.Entities;
using BookingGateway.Infrastructure.Repositories;
using DiscountGateway.Presentation.gRPC.Protos;
using Marten.Linq.Parsing;

namespace BookingGateway.Application.Features;

public record CreateBookingCommand(
    Guid CustomerId,
    string CustomerName,
    decimal TotalAmount,
    List<BookingService> Services) : ICommand<CreateBookingResponse>;

public record CreateBookingResponse(Guid CustomerId);

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer Id is required");
        RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer Name is required");
        RuleFor(x => x.TotalAmount).GreaterThan(0).WithMessage("Amount is required");
        RuleForEach(x => x.Services).NotNull().WithMessage("At least 1 offer service is required.");
    }
}

internal class CreateBookingHandler(IBookingRepository repository, DiscountService.DiscountServiceClient discountGrpc) : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<CreateBookingResponse> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
    {
        var booking = new Booking
        {
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName,
            Services = command.Services
        };

        // contact Discount.Grpc and get discount for product then calculate latest serfvice amount
        await ApplyDiscount(booking, cancellationToken);

        var result = await repository.CreateBookingAsync(booking);

        //return response
        return new CreateBookingResponse(result.CustomerId);
    } 

    // apply the discount if any exist for each offer service in Booking
    private async Task ApplyDiscount(Booking booking, CancellationToken cancellationToken)
    {
        foreach (var service in booking.Services)
        {
            // OfferId is the same as CouponId
            var discount = await discountGrpc.GetDiscountByIdAsync(new GetDiscountByIdRequest { Id = service.OfferId.ToString() }, cancellationToken: cancellationToken);
            service.Original = service.Amount;
            service.Discount = discount.Coupon.Amount;
            service.Amount -= discount.Coupon.Amount;
        }
    }
}