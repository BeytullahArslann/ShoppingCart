using System.Reflection;
using ShoppingCart.Application.Common.Interfaces;
using ShoppingCart.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<VasItem> VasItems => Set<VasItem>();

    public virtual void SetDetached(object entity)
    {
        Entry(entity).State = EntityState.Detached;
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Cart>()
            .HasMany(c => c.Items)
            .WithOne(c => c.Cart)
            .HasForeignKey(c => c.CartId);

        builder.Entity<CartItem>()
            .HasMany(c => c.VasItems)
            .WithOne(c => c.CartItem)
            .HasForeignKey(c => c.ItemId);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}