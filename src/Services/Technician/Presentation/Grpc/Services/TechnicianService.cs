using Grpc.Core;
using TechnicianGateway.Application.Features;
using TechnicianGateway.Domain.Entities;
using TechnicianGateway.Presentation.Grpc.Protos;

namespace TechnicianGateway.Presentation.Grpc.Services;

public class TechnicianService(IMediator mediator) : TechnicianProtoService.TechnicianProtoServiceBase
{
    public override async Task<GetAllTechniciansReply> GetAllTechnicians(GetAllTechniciansRequest request, ServerCallContext context)
    {
        // build command
        var command = new GetAllTechniciansQuery();

        // execute
        var result = await mediator.Send(command);

        //build grpc reply
        var reply = new GetAllTechniciansReply();

        foreach (var x in result.Technicians)
        {
            reply.Technicians.Add(new TechnicianProtoModel
            {
                Id = x.Id.ToString(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Phone = x.Phone is null ? string.Empty : x.Phone,
                DateOfBirth = x.DateOfBirth.ToShortDateString(),
                Gender = x.Gender,
                Address = x.Address is null ? string.Empty : x.Address,
                City = x.City is null ? string.Empty : x.City,
                State = x.State is null ? string.Empty : x.State,
                Zip = x.Zip,
                CreatedOn = x.CreatedOn.ToString(),
                UpdatedOn = x.UpdatedOn.ToString()
            });
        }

        reply.Message = StatusCode.OK.ToString();

        return reply;
    }

    public override async Task<GetTechnicianByIdReply> GetTechnicianById(GetTechnicianByIdRequest request, ServerCallContext context)
    {
        // build command
        var command = new GetTechnicianByIdQuery(Guid.Parse(request.Id));

        // execute
        var result = await mediator.Send(command);

        //build grpc reply
        var reply = new GetTechnicianByIdReply();

        if (result is not null && result.technician is not null)
        {
            reply.Technician = new TechnicianProtoModel
            {
                Id = result.technician.Id.ToString(),
                FirstName = result.technician.FirstName,
                LastName = result.technician.LastName,
                Email = result.technician.Email,
                Phone = result.technician.Phone is null ? string.Empty : result.technician.Phone,
                DateOfBirth = result.technician.DateOfBirth.ToShortDateString(),
                Gender = result.technician.Gender,
                Address = result.technician.Address is null ? string.Empty : result.technician.Address,
                City = result.technician.City is null ? string.Empty : result.technician.City,
                State = result.technician.State is null ? string.Empty : result.technician.State,
                Zip = result.technician.Zip,
                CreatedOn = result.technician.CreatedOn.ToString(),
                UpdatedOn = result.technician.UpdatedOn.ToString()
            };
        }


        reply.Message = StatusCode.OK.ToString();

        return reply;

        // throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }

    public override async Task<CreateTechnicianReply> CreateTechnician(CreateTechnicianRequest request, ServerCallContext context)
    {
        // var technician = request.Technician.Adapt<Technician>();

        if (request is null || request.Technician is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));
        }

        // Map proto model to command model
        var command = new CreateTechnicianCommand(
            request.Technician.FirstName,
            request.Technician.LastName,
            request.Technician.Phone,
            request.Technician.Email,
            DateTime.Parse(request.Technician.DateOfBirth),
            request.Technician.Gender,
            request.Technician.Address,
            request.Technician.City,
            request.Technician.State,
            request.Technician.Zip);

        var result = await mediator.Send(command);

        // Map business result to proto response
        return new CreateTechnicianReply
        {
            Id = result.Id.ToString(),
            Message = StatusCode.OK.ToString()
        };
    }

    public override async Task<UpdateTechnicianReply> UpdateTechnician(UpdateTechnicianRequest request, ServerCallContext context)
    {
        if (request is null || request.Technician is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));
        }

        var command = new UpdateTechnicianCommand(
            Guid.Parse(request.Technician.Id),
            request.Technician.FirstName,
            request.Technician.LastName,
            request.Technician.Phone,
            request.Technician.Email,
            DateTime.Parse(request.Technician.DateOfBirth),
            request.Technician.Gender,
            request.Technician.Address,
            request.Technician.City,
            request.Technician.State,
            request.Technician.Zip);

        var result = await mediator.Send(command);

        // Map proto model to command model
        return new UpdateTechnicianReply
        {
            Message = StatusCode.OK.ToString()
        };
    }
}