using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class CartItem
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    public string SkuId { get; set; } = string.Empty;

    [ForeignKey("SkuId")]
    public virtual Sku Sku { get; set; } = null!;

    public string? GoodsId { get; set; }

    [ForeignKey("GoodsId")]
    public virtual Goods? Goods { get; set; }

    [StringLength(200)]
    public string? GoodsName { get; set; }

    [StringLength(255)]
    public string? Picture { get; set; }

    [StringLength(100)]
    public string? AttrsText { get; set; }

    public int Count { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NowPrice { get; set; }

    public int Stock { get; set; } = 0;

    public bool Selected { get; set; } = true;

    public bool IsEffective { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
