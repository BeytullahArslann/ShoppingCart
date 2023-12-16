using ShoppingCart.Application.Carts.Commands.AddVasItemToItem;
using FluentAssertions;
using NUnit.Framework;

namespace ShoppingCart.Application.UnitTests.Common.Validations;

public class AddVasItemToItemValidationTests
{
    private readonly AddVasItemToItemValidator validator = new();
    
    
    [Test]
    [TestCase(2000, 3004, 3242, 1000, 2, 5003)]
    [TestCase(2001, 3005, 3242, 1000, 2, 5003)]
    public void AddVasItemToItem_ReturnSuccess(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
    
    [Test]
    [TestCase(0, 3004, 3242, 1000, 2, 5003)]
    public void AddVasItemToItem_ReturnFailForItemId(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("ItemId cannot be empty");
    }
    
    [Test]
    [TestCase(1001, 0, 3242, 1000, 2, 5003)]
    public void AddVasItemToItem_ReturnFailForVasItemId(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("VasItemId cannot be empty");
    }
    
    [Test]
    [TestCase(1001, 1, 0, 1000, 2, 5003)]
    public void AddVasItemToItem_ReturnFailForVasCategoryId(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors?[0]?.ErrorMessage.Should().Be("VasCategoryId cannot be empty");
        result.Errors?[1].ErrorMessage.Should().Be("VasCategoryId only can be 3242");
    }
    
    [Test]
    [TestCase(1001, 1, 3243, 1000, 2, 5003)]
    public void AddVasItemToItem_ReturnFailForVasCategoryIdOnlyCanBe3242(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?[0].ErrorMessage.Should().Be("VasCategoryId only can be 3242");
    }
    
    [Test]
    [TestCase(1001, 1, 3242, 1000, 2, 0)]
    public void AddVasItemToItem_ReturnFailForVasSellerId(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors?[0].ErrorMessage.Should().Be("VasSellerId cannot be empty");
        result.Errors?[1].ErrorMessage.Should().Be("VasSellerId only can be 5003");
    }
    
    [Test]
    [TestCase(1001, 1, 3242, 1000, 2, 5004)]
    public void AddVasItemToItem_ReturnFailForVasSellerIdOnlyCanBe5003(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?[0].ErrorMessage.Should().Be("VasSellerId only can be 5003");
    }
    
    [Test]
    [TestCase(1001, 1, 3242, 0, 2, 5003)]
    public void AddVasItemToItem_ReturnFailForPrice(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?[0].ErrorMessage.Should().Be("Price cannot be empty");
    }
    
    [Test]
    [TestCase(1001, 1, 3242, 1000, 0, 5003)]
    public void AddVasItemToItem_ReturnFailForQuantity(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?[0].ErrorMessage.Should().Be("Quantity cannot be empty");
    }
    
    [Test]
    [TestCase(1001, 1, 3242, 1000, 4, 5003)]
    public void AddVasItemToItem_ReturnFailForMaxVasItemCountIs3(int itemId, int vasItemId, int vasCategoryId, double price, int quantity, int vasSellerId)
    {
        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId,price, quantity);
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?[0].ErrorMessage.Should().Be("Max vas item count is 3");
    }

}