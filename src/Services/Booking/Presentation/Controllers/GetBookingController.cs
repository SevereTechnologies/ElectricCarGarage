using BookingGateway.Application.Features;

namespace BookingGateway.Presentation.Controllers;

public class GetBookingController : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/booking/{customerId}", async (Guid customerId, ISender sender) =>
        {
            var respnse = await sender.Send(new GetBookingQuery(customerId));

            return Results.Ok(respnse);
        })
        .WithName("Get Booking")
        .Produces<GetBookingResponse > (StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Booking");
    }
}