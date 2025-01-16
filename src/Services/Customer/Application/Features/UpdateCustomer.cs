using Microsoft.AspNetCore.Http.HttpResults;

namespace CustomerGateway.Application.Features;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string Phone,
    string Email,
    string Address,
    string City,
    string State,
    string Zip,
    string Note) : ICommand<UpdateCustomerResponse>;

public record UpdateCustomerResponse(bool IsSuccess, string Message);

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Customer Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
    }
}

internal class UpdateCustomerHandler(IDocumentSession session) : ICommandHandler<UpdateCustomerCommand, UpdateCustomerResponse>
{
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await session.LoadAsync<Customer>(command.Id, cancellationToken);

        if (customer is null)
        {
            return new UpdateCustomerResponse(false, "Not Found");
        }

        // update customer info
        customer.Id = command.Id;
        customer.Name = command.Name;
        customer.Phone = command.Phone;
        customer.Email = command.Email;
        customer.Address = command.Address;
        customer.City = command.City;
        customer.State = command.State;
        customer.Zip = command.Zip;
        customer.Note = command.Note;
        customer.UpdatedOn = DateTime.UtcNow;

        // track data
        session.Update(customer);

        // post to db
        await session.SaveChangesAsync(cancellationToken);

        // return result
        return new UpdateCustomerResponse(true, "Updated!");
    }
}

public class UpdateCustomerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/customers",
            async (UpdateCustomerCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return Results.Ok(response);
            })
            .WithName("UpdateCustomer WithName")
            .Produces<UpdateCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Customer WithSummary")
            .WithDescription("Update Customer WithDescription");
    }
}