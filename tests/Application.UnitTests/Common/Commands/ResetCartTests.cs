using ShoppingCart.Application.Carts.Commands.ResetCart;
using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace ShoppingCart.Application.UnitTests.Common.Commands;

// Using nunit
public class ResetCartTests
{
    private readonly Mock<IApplicationDbContext> _dbContext;

    private readonly ResetCartCommandHandler _handler;

    public ResetCartTests()
    {
        _dbContext = new Mock<IApplicationDbContext>();
        _handler = new ResetCartCommandHandler(_dbContext.Object);
    }

    [Test]
    public void AddItem_ReturnSuccess()
    {
        var mockResult = TestDataHelper.GetTestCartData();
        var vasItems = mockResult.Items?.SelectMany(x => x.VasItems).ToList();
        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup<DbSet<VasItem>>(x => x.VasItems)
            .ReturnsDbSet(vasItems);
        _dbContext.Setup<DbSet<CartItem>>(x => x.CartItems)
            .ReturnsDbSet(mockResult.Items);

        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var result = _handler.Handle(new ResetCartCommand(), CancellationToken.None).Result;

        result.Result.Should().BeTrue();
        result.Message.Should().Be("Reset cart successfully");
    }
}