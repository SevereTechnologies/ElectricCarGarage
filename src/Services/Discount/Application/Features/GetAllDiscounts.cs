using Mapster;

namespace DiscountGateway.Application.Features;

public record GetAllDiscountsQuery() : IQuery<GetAllDiscountsResponse>;

public record GetAllDiscountsResponse(IEnumerable<CouponDto> coupons);

internal class GetAllDiscountsHandler(IDocumentSession session, ILogger<GetAllDiscountsHandler> logger) : IQueryHandler<GetAllDiscountsQuery, GetAllDiscountsResponse>
{
    public async Task<GetAllDiscountsResponse> Handle(GetAllDiscountsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Called GetAllDiscountsHandler with {@query}", query);

        var coupons = await session.Query<Coupon>()
        .ToListAsync(cancellationToken);

        var couponModel = coupons.Adapt<IEnumerable<CouponDto>>();

        return new GetAllDiscountsResponse(couponModel);
    }
}