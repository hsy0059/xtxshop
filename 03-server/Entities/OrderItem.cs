using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class OrderItem
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    public string OrderId { get; set; } = string.Empty;

    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; } = null!;

    [Required]
    public string SkuId { get; set; } = string.Empty;

    public string? SpuId { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(100)]
    public string? AttrsText { get; set; }

    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal CurPrice { get; set; }

    [StringLength(255)]
    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
