using ShoppingCart.Application.Carts.Commands.AddItem;
using FluentAssertions;
using NUnit.Framework;

namespace ShoppingCart.Application.UnitTests.Common.Validations;

public class AddItemValidationTests
{
    private readonly AddItemValidator validator = new();

    [Test]
    [TestCase(2000, 3004, 1, 1000, 2)]
    [TestCase(2001, 21, 21, 1000, 2)]
    [TestCase(2002, 22, 22, 2000, 1)]
    public void AddItem_ReturnSuccess(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
    
    [Test]
    [TestCase(0, 3004, 1, 1000, 1)]
    public void AddItem_ReturnFailForItemId(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("ItemId cannot be empty");
    }
    
    [Test]
    [TestCase(1001, 0, 1, 1000, 5)]
    public void AddItem_ReturnFailForCategoryId(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("CategoryId cannot be empty");
    }
    
    [Test]
    [TestCase(1001, 3242, 1, 1000, 5)]
    public void AddItem_ReturnFailForCategoryVatItem(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("CategoryId cannot be 3242");
    }
    
    [Test]
    [TestCase(1001, 1, 0, 1000, 1)]
    public void AddItem_ReturnFailForSellerId(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("SellerId cannot be empty");
    }
    
    [Test]
    [TestCase(1001, 1, 5003, 1000, 1)]
    public void AddItem_ReturnFailForSellerVatItem(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("SellerId cannot be 5003");
    }
    
    [Test]
    [TestCase(1001, 1, 1, 1000, 11)]
    [TestCase(2001, 2, 21, 1000, 11)]
    [TestCase(2002, 3, 22, 2000, 11)]
    public void AddItem_ReturnFailForQuantity(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("Max item count is 10");
    }
    
    [Test] 
    [TestCase(1001, 7889, 1, 1000, 6)] 
    [TestCase(2001, 7889, 21, 1000, 6)] 
    [TestCase(2002, 7889, 22, 2000, 6)]
    public void AddItem_ReturnFailForQuantityDigital(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("Max digital item count is 5");
    }
    
    [Test]
    [TestCase(1001, 1, 1, 0, 1)]
    public void AddItem_ReturnFailForPrice(int itemId, int categoryId, int sellerId, double price, int quantity)
    {
        var command = new AddItemCommand(itemId,categoryId, sellerId, price, quantity);
            
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("Price cannot be empty");
    }
    
    
}