using TechnicianGateway.Domain.Entities;

namespace TechnicianGateway.Application.Features;

public record CreateTechnicianCommand(
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    DateTime DateOfBirth,
    string Gender,
    string Address,
    string City,
    string State,
    int Zip) : ICommand<CreateTechnicianResponse>;

public record CreateTechnicianResponse(Guid Id);

public class CreateTechnicianValidator : AbstractValidator<CreateTechnicianCommand>
{
    public CreateTechnicianValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("LastName is required");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.DateOfBirth).NotNull().LessThan(DateTime.Now.AddYears(17)).WithMessage("DOB is required and must be at least 17 years old.");
    }
}

internal class CreateTechnicianHandler(IDocumentSession session) : ICommandHandler<CreateTechnicianCommand, CreateTechnicianResponse>
{
    public async Task<CreateTechnicianResponse> Handle(CreateTechnicianCommand command, CancellationToken cancellationToken)
    {
        var technician = new Technician
        {
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Phone = command.Phone,
            Email = command.Email,
            DateOfBirth = command.DateOfBirth,
            Gender = command.Gender,
            Address = command.Address,
            City = command.City,
            State = command.State,
            Zip = command.Zip,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        //track data
        session.Store(technician);

        //post to database
        await session.SaveChangesAsync(cancellationToken);

        //return response
        return new CreateTechnicianResponse(technician.Id);
    }
}
