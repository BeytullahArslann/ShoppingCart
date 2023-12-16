namespace ShoppingCart.Domain.Entities;

public sealed record TotalPricePromotionDto(int IfEqualsPrice = 100, int DiscountPrice = 10);