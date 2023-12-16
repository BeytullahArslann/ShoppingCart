using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Application.Common.Models;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Carts.Commands.ResetCart;

//"itemId": int, "vasItemId":int, "vasCategoryId": int, "vasSellerId":int, "price":double, "quantity":int
public record ResetCartCommand : IRequest<ResultDto>;

public class ResetCartCommandHandler(IApplicationDbContext context) : IRequestHandler<ResetCartCommand, ResultDto>
{
    public async Task<ResultDto> Handle(ResetCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await context.Carts
            .Include(c => c.Items)!
            .ThenInclude(i => i.VasItems)
            .FirstOrDefaultAsync(cancellationToken) ?? new Cart();  
        
        context.CartItems.RemoveRange(cart.Items!);
        context.VasItems.RemoveRange(cart.Items!.SelectMany(x=>x.VasItems));
        
        cart.AppliedPromotionId = 0;
        cart.TotalDiscount = 0;

        context.Carts.Update(cart);
        await context.SaveChangesAsync(cancellationToken);

        return new ResultDto(true, "Reset cart successfully");
    }
}