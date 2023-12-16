using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShoppingCart.Domain.Entities;

public class CartItem : BaseEntity
{
    [Key] public int ItemId { get; set; }

    public double Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public int SellerId { get; set; }
    public ICollection<VasItem> VasItems { get; set; } = new List<VasItem>();

    [ForeignKey(nameof(Cart))]
    [JsonIgnore]
    public int CartId { get; set; }

    [JsonIgnore] public virtual Cart? Cart { get; set; }
}