using ShoppingCart.Application.Carts.Commands.AddItem;
using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace ShoppingCart.Application.UnitTests.Common.Commands;

// Using nunit
public class AddItemTests
{
    private readonly Mock<IApplicationDbContext> _dbContext;
    private readonly Mock<IDiscountService> _discountService;
    private readonly AddItemCommandHandler _handler;

    public AddItemTests()
    {
        _dbContext = new Mock<IApplicationDbContext>();
        _discountService = new Mock<IDiscountService>();
        _handler = new AddItemCommandHandler(_dbContext.Object, _discountService.Object);
    }

    [Test]
    [TestCase(1001, 3004, 1, 1000, 2, 1)]
    [TestCase(2001, 21, 21, 1000, 2, 1)]
    [TestCase(2002, 22, 22, 2000, 1, 1)]
    public async Task AddItem_ReturnSuccess(int itemId, int categoryId, int sellerId, double price, int quantity,
        int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData();
        var testCart = TestDataHelper.GetTestCartData();
        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));
        var existItem = testCart.Items?.ToList().FirstOrDefault(i => i.ItemId == itemId);
        if (existItem is null)
        {
            var testItem = new CartItem
            {
                ItemId = itemId,
                CategoryId = categoryId,
                SellerId = sellerId,
                Price = price,
                Quantity = quantity,
                CartId = cartId
            };
            testCart.Items?.Add(testItem);
        }
        else
        {
            existItem = new CartItem
            {
                ItemId = itemId,
                CategoryId = categoryId,
                SellerId = sellerId,
                Price = price,
                Quantity = existItem.Quantity + quantity,
                CartId = cartId
            };
        }

        var command = new AddItemCommand(itemId, categoryId, sellerId, price, quantity);

        _discountService.Setup(x => x.ApplyDiscount(mockResult))
            .Returns(new DiscountService().ApplyDiscount(testCart));

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeTrue();
        result.Message.Should().Be("Item added to cart successfully");
    }

    [Test]
    [TestCase(1001, 2, 1, 1000, 2)]
    [TestCase(1001, 3004, 2, 1000, 2)]
    [TestCase(1001, 3004, 1, 1001, 1)]
    public async Task AddItem_ReturnItemCannotBeAddedToCartWithDifferentProperties(int itemId, int categoryId,
        int sellerId, double price, int quantity)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddItemCommand(itemId, categoryId, sellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("Item cannot be added to cart with different properties");
    }

    [Test]
    [TestCase(2001, 21, 21, 1000, 2)]
    [TestCase(2002, 22, 22, 2000, 2)]
    [TestCase(2002, 22, 22, 2000, 1)]
    public async Task AddItem_ReturnYouCanAddMaxTenDifferentItemsToCart(int itemId, int categoryId, int sellerId,
        double price, int quantity)
    {
        var mockResult = TestDataHelper.GetTestCartData(true);

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddItemCommand(itemId, categoryId, sellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Result.Should().BeFalse();
        result.Message.Should().Be("You can add max 10 different items to cart");
    }

    [Test]
    [TestCase(5002, 7889, 52, 5002, 1)]
    [TestCase(5004, 7889, 54, 5002, 6)]
    public async Task Additem_ReturnYouCanAddMaxFiveSameItemsToCartFromCategoryDigital(int itemId, int categoryId,
        int sellerId, double price, int quantity)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddItemCommand(itemId, categoryId, sellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Result.Should().BeFalse();
        result.Message.Should().Be("You can add max 5 same items to cart from category 7889");
    }

    [Test]
    [TestCase(1001, 3004, 1, 1000, 10)]
    [TestCase(1002, 1001, 2, 2000, 9)]
    [TestCase(1003, 3, 3, 3000, 8)]
    public async Task Additem_ReturnYouCanAddMaxTenSameItemsToCart(int itemId, int categoryId, int sellerId,
        double price, int quantity)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddItemCommand(itemId, categoryId, sellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Result.Should().BeFalse();
        result.Message.Should().Be("You can add max 10 same items to cart");
    }

    [Test]
    [TestCase(1001, 3004, 1, 1000, 5)]
    [TestCase(2002, 22, 22, 2000, 8)]
    [TestCase(2002, 22, 22, 2000, 8)]
    public async Task Additem_ReturnYouCanAddMaxThirtyItemsToCart(int itemId, int categoryId, int sellerId,
        double price, int quantity)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddItemCommand(itemId, categoryId, sellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Result.Should().BeFalse();
        result.Message.Should().Be("You can add max 30 items to cart");
    }


    [Test]
    [TestCase(2005, 5, 5, 500000, 1, 1)]
    [TestCase(2003, 3, 3, 300000, 2, 1)]
    [TestCase(2002, 2, 2, 200000, 3, 1)]
    public async Task Additem_ReturnCartTotalPriceCannotBeMoreThanFiveHundredThousand(int itemId, int categoryId,
        int sellerId, double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData();
        var testCart = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var existItem = testCart.Items?.ToList().FirstOrDefault(i => i.ItemId == itemId);
        if (existItem is null)
        {
            var testItem = new CartItem
            {
                ItemId = itemId,
                CategoryId = categoryId,
                SellerId = sellerId,
                Price = price,
                Quantity = quantity,
                CartId = cartId
            };
            testCart.Items?.Add(testItem);
        }
        else
        {
            existItem = new CartItem
            {
                ItemId = itemId,
                CategoryId = categoryId,
                SellerId = sellerId,
                Price = price,
                Quantity = existItem.Quantity + quantity,
                CartId = cartId
            };
        }

        var command = new AddItemCommand(itemId, categoryId, sellerId, price, quantity);

        _discountService.Setup(x => x.ApplyDiscount(mockResult))
            .Returns(new DiscountService().ApplyDiscount(testCart));

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("Cart total price cannot be more than 500000");
    }
}