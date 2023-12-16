using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Services;

public class DiscountService : IDiscountService
{
    public Cart ApplyDiscount(Cart cart)
    {
        var totalPricePromotions = new TotalPricePromotionDto[]
        {
            new(50000, 2000),
            new(10000, 1000),
            new(5000, 500),
            new(500, 250)
        };

        double[] discounts =
        {
            cart.SellerPromotionDiscount(), cart.CategoryPromotionDiscount(),
            cart.TotalPricePromotionDiscount(totalPricePromotions)
        };

        var maxDiscount = 0d;
        var discountId = 0;

        foreach (var discount in discounts.Select((value, index) => new { index, value }))
        {
            if (!(discount.value > maxDiscount)) continue;

            maxDiscount = discount.value;
            discountId = discount.index switch
            {
                0 => 9909,
                1 => 5676,
                2 => 1232,
                _ => 0
            };
        }

        cart.TotalDiscount = maxDiscount;
        cart.AppliedPromotionId = discountId;

        return cart;
    }
}