using DiscountGateway.Presentation.gRPC.Protos;
using protoCoupon = DiscountGateway.Presentation.gRPC.Protos;
using Grpc.Core;
using Mapster;

namespace DiscountGateway.Presentation.gRPC.Services;

public class DiscountGrpcService(IMediator mediator) : DiscountService.DiscountServiceBase
{
    public override async Task<GetAllDiscountsReply> GetAllDiscounts(GetAllDiscountsRequest request, ServerCallContext context)
    {
        // build command
        var command = new GetAllDiscountsQuery();

        // execute
        var result = await mediator.Send(command);

        //var coupons = result.coupons.Adapt<protoCoupon.Coupon>();

        //build grpc reply
        var reply = new GetAllDiscountsReply();
        foreach (var x in result.coupons)
        {
            reply.Coupons.Add(new protoCoupon.Coupon
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Description = x.Description is null ? string.Empty : x.Description,
                Amount = x.Amount,
                Expirationdate = x.ExpirationDate.ToString()
            });
        }

        reply.Message = StatusCode.OK.ToString();

        return reply;
    }

    public override async Task<GetDiscountByIdReply> GetDiscountById(GetDiscountByIdRequest request, ServerCallContext context)
    {
        // build command
        var command = new GetDiscountByIdQuery(Guid.Parse(request.Id));

        // execute
        var result = await mediator.Send(command);

        //build grpc reply
        var reply = new GetDiscountByIdReply();

        if (result is not null && result.coupon is not null)
        {
            reply.Coupon = new protoCoupon.Coupon
            {
                Id = result.coupon.Id,
                Name = result.coupon.Name,
                Description = result.coupon.Description is null ? string.Empty : result.coupon.Description,
                Amount = result.coupon.Amount,
                Expirationdate = result.coupon.ExpirationDate
            };
        }

        reply.Message = StatusCode.OK.ToString();

        return reply;

        // throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
    }

    public override async Task<CreateDiscountReply> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        // var discount = request.Discount.Adapt<Discount>();

        if (request is null || request.Coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));
        }

        // Map proto model to command model (Coupon.Id = the OfferId)
        var command = new CreateDiscountCommand(
            Guid.Parse(request.Coupon.Id),
            request.Coupon.Name,
            request.Coupon.Description,
            request.Coupon.Amount);

        var result = await mediator.Send(command);

        // Map business result to proto response
        return new CreateDiscountReply
        {
            Id = result.Id.ToString(),
            Message = StatusCode.OK.ToString()
        };
    }

    public override async Task<DeleteDiscountReply> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        if (request is null || request.Id is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));
        }

        var command = new DeleteDiscountCommand(
            Guid.Parse(request.Id));

        var result = await mediator.Send(command);

        // Map proto model to command model
        return new DeleteDiscountReply
        {
            Message = StatusCode.OK.ToString()
        };
    }
}