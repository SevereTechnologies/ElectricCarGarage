using Google.Protobuf.Collections;
using Grpc.Core;
using OfferGateway.Application.Features;
using OfferingGateway.Application.Features;
using OfferingGateway.Presentation.Grpc.Protos;
using System.ComponentModel;

namespace OfferingGateway.Presentation.Grpc.Services;

public class OfferService(IMediator mediator) : OfferProtocService.OfferProtocServiceBase
{
    public override async Task<GetAllOffersReply> GetAllOffers(GetAllOffersRequest request, ServerCallContext context)
    {
        // build command
        var command = new GetAllOffersQuery();

        // execute
        var result = await mediator.Send(command);

        //build grpc reply
        var reply = new GetAllOffersReply();

        foreach (var x in result.Offers)
        {
            var newOffer = new OfferProtoModel
            {
                Id = x.Id.ToString(),
                ServiceName = x.ServiceName,
                Description = x.Description,
                Price = (float)x.Price,
                Active = x.Active
            };

            foreach (var category in x.Category)
            {
                newOffer.Category.Add(category);
            }

            // add to offers
            reply.Offers.Add(newOffer);
        }

        reply.Message = StatusCode.OK.ToString();

        return reply;
    }

    public override async Task<GetOfferByIdReply> GetOfferById(GetOfferByIdRequest request, ServerCallContext context)
    {
        // build command
        var command = new GetOfferByIdQuery(Guid.Parse(request.Id));

        // execute
        var result = await mediator.Send(command);

        //build grpc reply
        var reply = new GetOfferByIdReply();

        if (result is not null && result.offer is not null)
        {
            reply.Offer = new OfferProtoModel
            {
                Id = result.offer.Id.ToString(),
                ServiceName = result.offer.ServiceName,
                Description = result.offer.Description,
                Price = (float)result.offer.Price,
                Active = result.offer.Active,
                // Category = AddOffer(result.offer.Category)
            };
        }

        foreach(var category in result!.offer!.Category)
        {
            reply.Offer.Category.Add(category);
        }

        reply.Message = StatusCode.OK.ToString();

        return reply;

        // throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }

    public override async Task<CreateOfferReply> CreateOffer(CreateOfferRequest request, ServerCallContext context)
    {
        // var offer = request.Offer.Adapt<Offer>();

        if (request is null || request.Offer is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));
        }

        // Map proto model to command model
        var command = new CreateOfferCommand(
            request.Offer.ServiceName,
            request.Offer.Category.ToList(),
            request.Offer.Description,
            (decimal)request.Offer.Price);

        var result = await mediator.Send(command);

        // Map business result to proto response
        return new CreateOfferReply
        {
            Id = result.Id.ToString(),
            Message = StatusCode.OK.ToString()
        };
    }

    public override async Task<UpdateOfferReply> UpdateOffer(UpdateOfferRequest request, ServerCallContext context)
    {
        if (request is null || request.Offer is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));
        }

        var command = new UpdateOfferCommand(
            Guid.Parse(request.Offer.Id),
            request.Offer.ServiceName,
            request.Offer.Category.ToList(),
            request.Offer.Description,
            (decimal)request.Offer.Price,
            request.Offer.Active);

        var result = await mediator.Send(command);

        // Map proto model to command model
        return new UpdateOfferReply
        {
            Message = StatusCode.OK.ToString()
        };
    }
}