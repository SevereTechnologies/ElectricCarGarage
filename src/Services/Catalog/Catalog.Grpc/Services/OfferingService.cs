using Grpc.Core;
using Marten;
using System.Threading;

namespace Catalog.Grpc.Services;

public class OfferingService2(IDocumentSession session, ILogger<GreeterService> logger) : Offering.OfferingBase
{
    public override async Task<GetOfferingsReply> GetOfferings(GetOfferingsRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }

    public override async Task<GetOfferingByIdReply> GetOfferingById(GetOfferingByIdRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }

    public override async Task<CreateOfferingReply> CreateOffering(CreateOfferingRequest request, ServerCallContext context)
    {
        //var model = request.Offering;
        //var offering = new OfferingModel
        //{
        //    Id = Guid.NewGuid(),
        //    Name = model.Name,
        //    Price = model.Price,
        //    Categories = request.Offering.Categories.ToList(),
        //    Active = true,
        //    Description = model.Description
        //};

        ////track data
        //session.Store(offering);

        ////post to database
        //await session.SaveChangesAsync(cancellationToken);

        ////return response
        //return new CreateOfferingResponse(offering.Id);

        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }

    public override async Task<UpdateOfferingReply> UpdateOffering(UpdateOfferingRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }

    public override async Task<DeleteOfferingReply> DeleteOffering(DeleteOfferingRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }
}