using BookingGateway.Application.Features;

namespace BookingGateway.Presentation.Controllers;

public class DeleteBookingController : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/booking/{customerId}", async (Guid customerId, ISender sender) =>
        {
            var response = await sender.Send(new DeleteBookingCommand(customerId));

            return Results.Ok(response);
        })
        .WithName("DeleteBooking")
        .Produces<DeleteBookingResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Booking");
    }
}