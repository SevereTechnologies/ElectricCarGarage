using Mapster;

namespace DiscountGateway.Application.Features;

public record GetDiscountByIdQuery(Guid Id) : IQuery<GetDiscountByIdResponse>;

public record GetDiscountByIdResponse(CouponDto? coupon);

internal class GetDiscountByIdHandler(IDocumentSession session) : IQueryHandler<GetDiscountByIdQuery, GetDiscountByIdResponse>
{
    public async Task<GetDiscountByIdResponse> Handle(GetDiscountByIdQuery query, CancellationToken cancellationToken)
    {
        var coupon = await session.LoadAsync<Coupon>(query.Id, cancellationToken);

        var couponDto = coupon.Adapt<CouponDto>();

        return new GetDiscountByIdResponse(couponDto);
    }
}
