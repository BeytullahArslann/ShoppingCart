using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShoppingCart.Domain.Entities;

public class VasItem : BaseEntity
{
    [ForeignKey(nameof(CartItem))] public int ItemId { get; set; }

    public double Price { get; set; }
    public int Quantity { get; set; }

    [Key] public int VasItemId { get; set; }

    public int VasCategoryId { get; set; }
    public int VasSellerId { get; set; }

    [JsonIgnore] public virtual CartItem? CartItem { get; set; }
}