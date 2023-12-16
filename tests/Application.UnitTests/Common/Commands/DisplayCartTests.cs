using ShoppingCart.Application.Carts.Commands.DisplayCart;
using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace ShoppingCart.Application.UnitTests.Common.Commands;

// Using nunit
public class DisplayCartTests
{
    private readonly Mock<IApplicationDbContext> _dbContext;
    private readonly DisplayCartCommandHandler _handler;

    public DisplayCartTests()
    {
        _dbContext = new Mock<IApplicationDbContext>();
        _handler = new DisplayCartCommandHandler(_dbContext.Object);
    }

    [Test]
    public void AddItem_ReturnSuccess()
    {
        var mockResult = TestDataHelper.GetTestCartData(true);
        _dbContext.Setup<DbSet<Cart>>(x => x.Carts)
            .ReturnsDbSet(new[] { mockResult });
        _dbContext.Setup(x => x.Carts.Entry(mockResult));

        var result = _handler.Handle(new DisplayCartCommand(), CancellationToken.None).Result;

        result.Should().NotBeNull();
        result.Should().BeSameAs(mockResult);
    }
}