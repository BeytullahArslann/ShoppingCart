using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Application.Common.Models;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Carts.Commands.RemoveItem;

//"itemId": int, "vasItemId":int, "vasCategoryId": int, "vasSellerId":int, "price":double, "quantity":int
public record RemoveItemCommand(int ItemId) : IRequest<ResultDto>;

public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, ResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IDiscountService _discountService;

    public RemoveItemCommandHandler(IApplicationDbContext context, IDiscountService discountService)
    {
        _context = context;
        _discountService = discountService;
    }

    public async Task<ResultDto> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var item = _context.CartItems
            .Include(x => x.VasItems)
            .FirstOrDefault(i => i.ItemId == request.ItemId);
        if (item == null) return new ResultDto(false, "Item not found");

        _context.CartItems.Remove(item);
        _context.VasItems.RemoveRange(item.VasItems);

        await _context.SaveChangesAsync(cancellationToken);

        var cart = _context.Carts.FirstOrDefault() ?? new Cart();

        cart = _discountService.ApplyDiscount(cart);
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync(cancellationToken);

        return new ResultDto(true, "Item deleted successfully");
    }
}