using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Application.Common.Models;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Carts.Commands.AddVasItemToItem;

public record AddVasItemToItemCommand(
    int ItemId,
    int VasItemId,
    int VasCategoryId,
    int VasSellerId,
    double Price,
    int Quantity) : IRequest<ResultDto>;

public class AddVasItemToItemCommandHandler(IApplicationDbContext context, IDiscountService discountService)
    : IRequestHandler<AddVasItemToItemCommand, ResultDto>
{
    public async Task<ResultDto> Handle(AddVasItemToItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await context.Carts
            .Include(c => c.Items)!
            .ThenInclude(i => i.VasItems)
            .FirstOrDefaultAsync(cancellationToken) ?? new Cart();

        context.SetDetached(cart);

        var item = cart.Items?.FirstOrDefault(i => i.ItemId == request.ItemId);
        if (item == null)
            return new ResultDto(false, "Item not found");

        if (request.Price > item.Price)
            return new ResultDto(false, "Vas Item price cannot be greater than Item price");


        if (item.CategoryId != 1001 && item.CategoryId != 3004)
            return new ResultDto(false, "Vas Item cannot be added to this item category");

        var requestItem = context.VasItems.FirstOrDefault(i => i.VasItemId == request.VasItemId);
        if (requestItem is not null && (requestItem?.VasCategoryId != request.VasCategoryId ||
                                        requestItem.VasSellerId != request.VasSellerId ||
                                        requestItem.Price != request.Price))
            return new ResultDto(false, "Item cannot be added to cart with different properties");

        var newVasItem = new VasItem
        {
            ItemId = request.ItemId,
            VasCategoryId = request.VasCategoryId,
            VasSellerId = request.VasSellerId,
            Price = request.Price,
            Quantity = request.Quantity,
            VasItemId = request.VasItemId
        };
        if (requestItem is null)
        {
            item.VasItems?.Add(newVasItem);
        }
        else
        {
            requestItem.Quantity += request.Quantity;
        }

        var vasItemCount = item?.VasItems?.Sum(x => x.Quantity) ?? 0;
        var totalItemCount = cart.Items?
            .Sum(i => i.Quantity + i.VasItems.Count) ?? 0;

        if (vasItemCount > 3)
            return new ResultDto(false, "You can add max 3 vas items to item");

        if (totalItemCount > 30)
            return new ResultDto(false, "You can add max 30 items to cart");


        cart = discountService.ApplyDiscount(cart);

        if (cart.TotalPrice > 500000)
            return new ResultDto(false, "Cart total price cannot be more than 500000");


        if (requestItem is null)
        {
            context.VasItems.Add(newVasItem);
        }
        else
        {
            context.VasItems.Update(requestItem);
        }

        await context.SaveChangesAsync(cancellationToken);

        var data = context.Carts.FirstOrDefault() ?? new Cart();
        data.AppliedPromotionId = cart.AppliedPromotionId;
        data.TotalDiscount = cart.TotalDiscount;
        context.Carts.Update(data);
        await context.SaveChangesAsync(cancellationToken);

        return new ResultDto(true, "Vas Item added to Item successfully");
    }
}