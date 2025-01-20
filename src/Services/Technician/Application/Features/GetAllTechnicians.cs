using TechnicianGateway.Domain.Entities;

namespace TechnicianGateway.Application.Features;

public record GetAllTechniciansQuery() : IQuery<GetAllTechniciansResponse>;

public record GetAllTechniciansResponse(IEnumerable<Technician> Technicians);

internal class GetAllTechniciansHandler(IDocumentSession session, ILogger<GetAllTechniciansHandler> logger) : IQueryHandler<GetAllTechniciansQuery, GetAllTechniciansResponse>
{
    public async Task<GetAllTechniciansResponse> Handle(GetAllTechniciansQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Called GetAllTechniciansHandler with {@query}", query);

        var technicians = await session.Query<Technician>()
            .ToListAsync(cancellationToken);

        return new GetAllTechniciansResponse(technicians);
    }
}


