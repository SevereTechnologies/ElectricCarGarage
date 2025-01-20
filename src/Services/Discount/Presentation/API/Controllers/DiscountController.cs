namespace DiscountGateway.Presentation.API.Controllers;

public class CreateDiscountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/discounts",
            async (CreateDiscountCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Created($"/discounts/{response.Id}", response);
            })
        .WithName("CreateDiscount")
        .Produces<CreateDiscountResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Discount");
    }
}

public class DeleteDiscountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/discounts",
            async (DeleteDiscountCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Ok(response);
            })
            .WithName("DeleteDiscount WithName")
            .Produces<DeleteDiscountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Discount WithSummary")
            .WithDescription("Delete Discount WithDescription");
    }
}

public class GetAllDiscountsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/discounts", async (ISender sender) =>
        {
            var response = await sender.Send(new GetAllDiscountsQuery());

            return Results.Ok(response);
        })
        .WithName("GetAllDiscounts")
        .Produces<GetAllDiscountsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Discounts");
    }
}

public class GetDiscountByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/discounts/{id}", async (Guid id, ISender sender) =>
        {
            var respnse = await sender.Send(new GetDiscountByIdQuery(id));

            return Results.Ok(respnse);
        })
        .WithName("GetDiscountById")
        .Produces<GetDiscountByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Discount By Id");
    }
}