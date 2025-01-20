namespace TechnicianGateway.Domain.Entities;

public record Technician
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; } = default!;
    public required string LastName { get; set; } = default!;
    public required string Email { get; set; } = default!;
    public required string Phone { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public int Zip { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = default!;
    public DateTime UpdatedOn { get; set; } = default!;
}
