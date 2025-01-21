using OfferGateway.Application.Features;
using OfferingGateway.Application.Features;

namespace OfferGateway.Presentation.API.Controllers;

public class CreateOfferEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/offers",
            async (CreateOfferCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Created($"/offers/{response.Id}", response);
            })
        .WithName("CreateOffer")
        .Produces<CreateOfferResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Offer");
    }
}

public class UpdateOfferEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/offers",
            async (UpdateOfferCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Ok(response);
            })
            .WithName("UpdateOffer WithName")
            .Produces<UpdateOfferResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Offer WithSummary")
            .WithDescription("Update Offer WithDescription");
    }
}

public class GetAllOffersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/offers", async (ISender sender) =>
        {
            var response = await sender.Send(new GetAllOffersQuery());

            return Results.Ok(response);
        })
        .WithName("GetAllOffers")
        .Produces<GetAllOffersResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Offers");
    }
}

public class GetOfferByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/offers/{id}", async (Guid id, ISender sender) =>
        {
            var respnse = await sender.Send(new GetOfferByIdQuery(id));

            return Results.Ok(respnse);
        })
        .WithName("GetOfferById")
        .Produces<GetOfferByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Offer By Id");
    }
}