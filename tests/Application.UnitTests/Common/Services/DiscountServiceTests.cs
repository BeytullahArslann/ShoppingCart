using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure.Services;
using FluentAssertions;
using NUnit.Framework;

namespace ShoppingCart.Application.UnitTests.Common.Services;

public class DiscountServiceTests
{
    private readonly DiscountService service = new();
    private readonly Cart cart = TestDataHelper.GetTestCartData(true);
    
    
    [Test]
    public void GetDiscount_ReturnSuccess()
    {
        var result = service.ApplyDiscount(cart);
        result.AppliedPromotionId.Should().Be(1232);
        result.TotalDiscount.Should().Be(2000.0d);
    }
    
}