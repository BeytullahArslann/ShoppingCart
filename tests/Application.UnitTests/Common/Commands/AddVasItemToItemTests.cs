using ShoppingCart.Application.Carts.Commands.AddVasItemToItem;
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
public class AddVasItemToItemTests
{
    private readonly Mock<IApplicationDbContext> _dbContext;
    private readonly Mock<IDiscountService> _discountService;
    private readonly AddVasItemToItemCommandHandler _handler;

    public AddVasItemToItemTests()
    {
        _dbContext = new Mock<IApplicationDbContext>();
        _discountService = new Mock<IDiscountService>();
        _handler = new AddVasItemToItemCommandHandler(_dbContext.Object, _discountService.Object);
    }

    [Test]
    [TestCase(1001, 7001, 3242, 5003, 100, 1, 1)]
    [TestCase(1002, 7002, 3242, 5003, 100, 2, 1)]
    public async Task AddVasItemToItem_ReturnSuccess(int itemId, int vasItemId, int vasCategoryId, int vasSellerId,
        double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData();
        var testCart = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });

        _dbContext.Setup<DbSet<VasItem>>(x => x.VasItems)
            .ReturnsDbSet(mockResult.Items?.SelectMany(i => i.VasItems));

        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var existItem = testCart.Items?.FirstOrDefault(i => i.ItemId == itemId);
        var existVasItem = existItem?.VasItems?.FirstOrDefault(i => i.VasItemId == vasItemId);
        if (existVasItem is null)
        {
            var testItem = new VasItem
            {
                ItemId = itemId,
                VasCategoryId = vasCategoryId,
                VasSellerId = vasSellerId,
                Price = price,
                Quantity = quantity,
                VasItemId = vasItemId
            };
            existItem?.VasItems?.Add(testItem);
        }
        else
        {
            existVasItem = new VasItem
            {
                ItemId = itemId,
                VasCategoryId = vasCategoryId,
                VasSellerId = vasSellerId,
                Price = price,
                Quantity = existVasItem.Quantity + quantity,
                VasItemId = vasItemId
            };
        }

        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId, price, quantity);

        _discountService.Setup(x => x.ApplyDiscount(mockResult))
            .Returns(new DiscountService().ApplyDiscount(testCart));

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeTrue();
        result.Message.Should().Be("Vas Item added to Item successfully");
    }

    [Test]
    [TestCase(9898, 7001, 3242, 5003, 100, 1, 1)]
    [TestCase(9890, 7002, 3242, 5003, 100, 2, 1)]
    public async Task AddVasItemToItem_ReturnItemNotFound(int itemId, int vasItemId, int vasCategoryId, int vasSellerId,
        double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("Item not found");
    }

    [Test]
    [TestCase(1001, 7001, 3242, 5003, 10000, 1, 1)]
    [TestCase(1002, 7002, 3242, 5003, 2001, 2, 1)]
    public async Task AddVasItemToItem_ReturnVasItemPriceCannotBeGreaterThanItemPrice(int itemId, int vasItemId,
        int vasCategoryId, int vasSellerId, double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("Vas Item price cannot be greater than Item price");
    }

    [Test]
    [TestCase(1003, 7001, 3242, 5003, 200, 1, 1)]
    [TestCase(1004, 7002, 3242, 5003, 300, 2, 1)]
    [TestCase(1005, 7002, 3242, 5003, 400, 2, 1)]
    public async Task AddVasItemToItem_ReturnVasItemCannotBeAddedToThisItemCategory(int itemId, int vasItemId,
        int vasCategoryId, int vasSellerId, double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("Vas Item cannot be added to this item category");
    }

    [Test]
    [TestCase(1001, 7001, 3242, 5003, 101, 1, 1)]
    public async Task AddVasItemToItem_ReturnItemCannotBeAddedToCartWithDifferentProperties(int itemId, int vasItemId,
        int vasCategoryId, int vasSellerId, double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup<DbSet<VasItem>>(x => x.VasItems)
            .ReturnsDbSet(mockResult.Items?.SelectMany(i => i.VasItems));
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("Item cannot be added to cart with different properties");
    }


    [Test]
    [TestCase(1001, 7001, 3242, 5003, 100, 2, 1)]
    [TestCase(1001, 7002, 3242, 5003, 100, 2, 1)]
    [TestCase(1001, 7003, 3242, 5003, 500, 2, 1)]
    public async Task AddVasItemToItem_ReturnYouCanAddMaxThreeVasItemsToCart(int itemId, int vasItemId,
        int vasCategoryId, int vasSellerId, double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup<DbSet<VasItem>>(x => x.VasItems)
            .ReturnsDbSet(mockResult.Items?.SelectMany(i => i.VasItems));
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("You can add max 3 vas items to item");
    }

    [Test]
    [TestCase(1002, 7001, 3242, 5003, 100, 3, 1)]
    [TestCase(1002, 7002, 3242, 5003, 100, 3, 1)]
    [TestCase(1002, 7003, 3242, 5003, 500, 3, 1)]
    public async Task AddVasItemToItem_ReturnYouCanAddMaxThirtyItemsToCart(int itemId, int vasItemId, int vasCategoryId,
        int vasSellerId, double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartData(true);

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup<DbSet<VasItem>>(x => x.VasItems)
            .ReturnsDbSet(mockResult.Items?.SelectMany(i => i.VasItems));
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId, price, quantity);

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("You can add max 30 items to cart");
    }

    [Test]
    [TestCase(1001, 7001, 3242, 5003, 6000, 3, 1)]
    [TestCase(1001, 7002, 3242, 5003, 6000, 2, 1)]
    [TestCase(1001, 7003, 3242, 5003, 2000, 3, 1)]
    public async Task AddVasItemToItem_ReturnCartTotalPriceCannotBeMoreThanFiveHundredThousand(int itemId,
        int vasItemId, int vasCategoryId, int vasSellerId, double price, int quantity, int cartId)
    {
        var mockResult = TestDataHelper.GetTestCartDataFullAmount();
        var testCart = TestDataHelper.GetTestCartDataFullAmount();

        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });

        _dbContext.Setup<DbSet<VasItem>>(x => x.VasItems)
            .ReturnsDbSet(mockResult.Items?.SelectMany(i => i.VasItems));

        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var existItem = testCart.Items?.FirstOrDefault(i => i.ItemId == itemId);
        var existVasItem = existItem?.VasItems?.FirstOrDefault(i => i.VasItemId == vasItemId);
        if (existVasItem is null)
        {
            var testItem = new VasItem
            {
                ItemId = itemId,
                VasCategoryId = vasCategoryId,
                VasSellerId = vasSellerId,
                Price = price,
                Quantity = quantity,
                VasItemId = vasItemId
            };
            existItem?.VasItems?.Add(testItem);
        }
        else
        {
            existVasItem = new VasItem
            {
                ItemId = itemId,
                VasCategoryId = vasCategoryId,
                VasSellerId = vasSellerId,
                Price = price,
                Quantity = existVasItem.Quantity + quantity,
                VasItemId = vasItemId
            };
        }

        var command = new AddVasItemToItemCommand(itemId, vasItemId, vasCategoryId, vasSellerId, price, quantity);

        _discountService.Setup(x => x.ApplyDiscount(mockResult))
            .Returns(new DiscountService().ApplyDiscount(testCart));

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
        result.Message.Should().Be("Cart total price cannot be more than 500000");
    }
}