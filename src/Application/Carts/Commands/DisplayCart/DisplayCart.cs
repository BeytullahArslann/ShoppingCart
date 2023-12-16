using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Carts.Commands.DisplayCart;

public record DisplayCartCommand : IRequest<Cart>;

public class DisplayCartCommandHandler : IRequestHandler<DisplayCartCommand, Cart>
{
    private readonly IApplicationDbContext _context;

    public DisplayCartCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> Handle(DisplayCartCommand request, CancellationToken cancellationToken)
    {
        return await _context.Carts
            .Include(c => c.Items)!
            .ThenInclude(i => i.VasItems)
            .FirstOrDefaultAsync(cancellationToken) ?? new Cart();
    }
}