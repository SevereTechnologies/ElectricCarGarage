using TechnicianGateway.Domain.Entities;

namespace TechnicianGateway.Application.Features;

public record GetTechnicianByIdQuery(Guid Id) : IQuery<GetTechnicianByIdResponse>;

public record GetTechnicianByIdResponse(Technician? technician);

internal class GetTechnicianByIdHandler(IDocumentSession session) : IQueryHandler<GetTechnicianByIdQuery, GetTechnicianByIdResponse>
{
    public async Task<GetTechnicianByIdResponse> Handle(GetTechnicianByIdQuery query, CancellationToken cancellationToken)
    {
        var technician = await session.LoadAsync<Technician>(query.Id, cancellationToken);

        return new GetTechnicianByIdResponse(technician);
    }
}
