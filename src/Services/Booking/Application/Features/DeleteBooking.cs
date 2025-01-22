using BookingGateway.Infrastructure.Repositories;

namespace BookingGateway.Application.Features;

public record DeleteBookingCommand(Guid CustomerId) : ICommand<DeleteBookingResponse>;

public record DeleteBookingResponse(bool IsSuccess);

public class DeleteBookingValidator : AbstractValidator<DeleteBookingCommand>
{
    public DeleteBookingValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required");
    }
}

internal class DeleteBookingHandler(IBookingRepository repository) : ICommandHandler<DeleteBookingCommand, DeleteBookingResponse>
{
    public async Task<DeleteBookingResponse> Handle(DeleteBookingCommand command, CancellationToken cancellationToken)
    {
        // delete booking from database and cache
        await repository.DeleteBookingAsync(command.CustomerId, cancellationToken);

        return new DeleteBookingResponse(true);
    }
}