
using TechnicianGateway.Application.Features;

namespace TechnicianGateway.Presentation.API.Controllers;

public class CreateTechnicianEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/technicians",
            async (CreateTechnicianCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Created($"/technicians/{response.Id}", response);
            })
        .WithName("CreateTechnician")
        .Produces<CreateTechnicianResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Technician");
    }
}

public class UpdateTechnicianEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/technicians",
            async (UpdateTechnicianCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Ok(response);
            })
            .WithName("UpdateTechnician WithName")
            .Produces<UpdateTechnicianResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Technician WithSummary")
            .WithDescription("Update Technician WithDescription");
    }
}

public class GetAllTechniciansEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/technicians", async (ISender sender) =>
        {
            var response = await sender.Send(new GetAllTechniciansQuery());

            return Results.Ok(response);
        })
        .WithName("GetAllTechnicians")
        .Produces<GetAllTechniciansResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Technicians");
    }
}

public class GetTechnicianByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/technicians/{id}", async (Guid id, ISender sender) =>
        {
            var respnse = await sender.Send(new GetTechnicianByIdQuery(id));

            return Results.Ok(respnse);
        })
        .WithName("GetTechnicianById")
        .Produces<GetTechnicianByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Technician By Id");
    }
}