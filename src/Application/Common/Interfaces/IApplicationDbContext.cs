using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }
    DbSet<VasItem> VasItems { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    void SetDetached(object entity);
}