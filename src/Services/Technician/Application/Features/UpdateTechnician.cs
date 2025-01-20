using TechnicianGateway.Domain.Entities;

namespace TechnicianGateway.Application.Features;

public record UpdateTechnicianCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    DateTime DateOfBirth,
    string Gender,
    string Address,
    string City,
    string State,
    int Zip) : ICommand<UpdateTechnicianResponse>;

public record UpdateTechnicianResponse(bool IsSuccess, string Message);

public class UpdateTechnicianValidator : AbstractValidator<UpdateTechnicianCommand>
{
    public UpdateTechnicianValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("LastName is required");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.DateOfBirth).NotNull().LessThan(DateTime.Now.AddYears(17)).WithMessage("DOB is required and must be at least 17 years old.");
    }
}

internal class UpdateTechnicianHandler(IDocumentSession session) : ICommandHandler<UpdateTechnicianCommand, UpdateTechnicianResponse>
{
    public async Task<UpdateTechnicianResponse> Handle(UpdateTechnicianCommand command, CancellationToken cancellationToken)
    {
        var technician = await session.LoadAsync<Technician>(command.Id, cancellationToken);

        if (technician is null)
        {
            return new UpdateTechnicianResponse(false, "Not Found");
        }

        // update technician info
        technician.Id = command.Id;
        technician.FirstName = command.FirstName;
        technician.LastName = command.LastName;
        technician.Phone = command.Phone;
        technician.Email = command.Email;
        technician.Gender = command.Gender;
        technician.DateOfBirth = command.DateOfBirth;
        technician.Address = command.Address;
        technician.City = command.City;
        technician.State = command.State;
        technician.Zip = command.Zip;
        technician.UpdatedOn = DateTime.UtcNow;

        // track data
        session.Update(technician);

        // post to db
        await session.SaveChangesAsync(cancellationToken);

        // return result
        return new UpdateTechnicianResponse(true, "Updated!");
    }
}
