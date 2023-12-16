using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Services;

public interface IDiscountService
{
    public Cart ApplyDiscount(Cart cart);
}