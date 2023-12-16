using ShoppingCart.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace ShoppingCart.Domain.UnitTests.ValueObjects;

public class CartTests
{
    [Test]
    public void Card_TotalAmount_ReturnTotal()
    {
        var cart = TestDataHelper.GetTestCartData();
        cart.TotalAmount.Should().Be(129224.0d);
    }
    
    [Test]
    public void Card_SellerPromotionDiscount_ReturnDiscount()
    {
        var cart = TestDataHelper.GetTestCartData_SameSeller();
        cart.SellerPromotionDiscount().Should().Be(7420.0d);
    }
    
    [Test]
    public void Card_CategoryPromotionDiscount_ReturnDiscount()
    {
        var cart = TestDataHelper.GetTestCartData();
        cart.CategoryPromotionDiscount().Should().Be(450.0d);
    }
    
    
    [Test]
    public void Card_TotalPricePromotionDiscount_ReturnDiscount()
    {
        var cart = TestDataHelper.GetTestCartData();

        var totalPricePromotions = new[]
        {
            new TotalPricePromotionDto(50000, 2000),
            new TotalPricePromotionDto(10000, 1000),
            new TotalPricePromotionDto(5000, 500),
            new TotalPricePromotionDto(500, 250)
        };
        
        cart.TotalPricePromotionDiscount(totalPricePromotions).Should().Be(2000);
    }
}