using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Application.Common.Models;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Carts.Commands.AddItem;

public record AddItemCommand(int ItemId, int CategoryId, int SellerId, double Price, int Quantity)
    : IRequest<ResultDto>;

public class AddItemCommandHandler(IApplicationDbContext context, IDiscountService discountService)
    : IRequestHandler<AddItemCommand, ResultDto>
{
    public async Task<ResultDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await context.Carts
            .Include(c => c.Items)!
            .ThenInclude(i => i.VasItems)
            .FirstOrDefaultAsync(cancellationToken) ?? new Cart();


        context.SetDetached(cart);

        var requestItem = cart.Items?.FirstOrDefault(i => i.ItemId == request.ItemId);
        if (requestItem is not null && (requestItem?.CategoryId != request.CategoryId ||
                                        requestItem.SellerId != request.SellerId || requestItem.Price != request.Price))
            return new ResultDto(false, "Item cannot be added to cart with different properties");

        var newItem = new CartItem
        {
            ItemId = request.ItemId,
            CategoryId = request.CategoryId,
            SellerId = request.SellerId,
            Price = request.Price,
            Quantity = request.Quantity,
            CartId = 1
        };
        if (requestItem is null)
        {
            cart.Items?.Add(newItem);
        }
        else
        {
            requestItem.Quantity += request.Quantity;
        }

        var itemCount = cart.Items?.Count() ?? 0;
        var requestItemCount = requestItem?.Quantity ?? newItem.Quantity;
        var totalItemCount = cart.Items?
            .Sum(i => i.Quantity + i.VasItems.Count) ?? 0;

        if (itemCount > 10)
            return new ResultDto(false, "You can add max 10 different items to cart");

        if (request.CategoryId == 7889 && requestItemCount > 5)
            return new ResultDto(false, "You can add max 5 same items to cart from category 7889");

        if (requestItemCount > 10)
            return new ResultDto(false, "You can add max 10 same items to cart");

        if (totalItemCount > 30)
            return new ResultDto(false, "You can add max 30 items to cart");


        cart = discountService.ApplyDiscount(cart);

        if (cart.TotalPrice > 500000)
            return new ResultDto(false, "Cart total price cannot be more than 500000");


        if (requestItem == null)
        {
            context.CartItems.Add(newItem);
        }
        else
        {
            context.CartItems.Update(requestItem);
        }

        await context.SaveChangesAsync(cancellationToken);

        var data = context.Carts.FirstOrDefault() ?? new Cart();
        data.AppliedPromotionId = cart.AppliedPromotionId;
        data.TotalDiscount = cart.TotalDiscount;
        context.Carts.Update(data);
        await context.SaveChangesAsync(cancellationToken);

        return new ResultDto(true, "Item added to cart successfully");
    }
}