using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShoppingCart.Domain.Entities;

public class Cart : BaseEntity
{
    [Key] [JsonIgnore] public int Id { get; set; }

    public ICollection<CartItem>? Items { get; set; } = new List<CartItem>();

    public double TotalAmount
    {
        get { return Items?.Sum(x => (x.VasItems?.Sum(y => y.Quantity * y.Price) ?? 0) + x.Quantity * x.Price) ?? 0; }
    }

    [JsonIgnore] public double TotalPrice => TotalAmount - TotalDiscount;

    public int AppliedPromotionId { get; set; }

    public double TotalDiscount { get; set; }

    public virtual double SellerPromotionDiscount(double discount = 10)
    {
        var vasItemSellers = Items?
            .SelectMany(x => x.VasItems?.Select(y => y.VasSellerId) ?? Array.Empty<int>())
            .Distinct()
            .ToArray();

        var itemSellers = Items?
            .Select(x => x.SellerId)
            .Distinct()
            .ToArray();

        var totalSellers = Array.Empty<int>()
            .Concat(vasItemSellers ?? Array.Empty<int>())
            .Concat(itemSellers ?? Array.Empty<int>())
            .Distinct();

        if (totalSellers.Count() == 1) return TotalAmount / 100 * discount;

        return 0;
    }

    public virtual double CategoryPromotionDiscount(double discount = 5, int categoryId = 3003)
    {
        return Items?.Where(x => x.CategoryId == categoryId).Sum(x => x.Price / 100d * discount) ?? 0;
    }

    public virtual double TotalPricePromotionDiscount(TotalPricePromotionDto[] totalPricePromotions)
    {
        var maxPromotion = totalPricePromotions
            .Where(x => x.IfEqualsPrice <= TotalAmount)
            .MaxBy(x => x.IfEqualsPrice);

        return maxPromotion?.DiscountPrice ?? 0;
    }
}