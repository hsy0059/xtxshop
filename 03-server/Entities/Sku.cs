using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class Sku
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    public string GoodsId { get; set; } = string.Empty;

    [ForeignKey("GoodsId")]
    public virtual Goods Goods { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string SkuCode { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OldPrice { get; set; }

    public int Inventory { get; set; } = 0;

    [StringLength(255)]
    public string? Picture { get; set; }

    public string? Specs { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
