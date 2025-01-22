using BookingGateway.Application.Features;

namespace BookingGateway.Presentation.Controllers;

public class CreateBookingEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/booking", async (CreateBookingCommand command, ISender sender) =>
        {
            var response = await sender.Send(command);

            return Results.Created($"/booking/{response.CustomerId}", response);
        })
        .WithName("CreateBooking")
        .Produces<CreateBookingResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Booking");
    }
}