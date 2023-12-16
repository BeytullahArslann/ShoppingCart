using ShoppingCart.Application.Carts.Commands.RemoveItem;
using FluentAssertions;
using NUnit.Framework;

namespace ShoppingCart.Application.UnitTests.Common.Validations;

public class RemoveItemValidationTests
{
    private readonly RemoveItemValidator validator = new();
    
    [Test]
    [TestCase(2000)]
    [TestCase(2001)]
    public void RemoveItem_ReturnSuccess(int itemId)
    {
        var command = new RemoveItemCommand(itemId);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
    
    [Test]
    [TestCase(0)]
    public void RemoveItem_ReturnFailForItemId(int itemId)
    {
        var command = new RemoveItemCommand(itemId);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("ItemId cannot be empty");
    }
    
}