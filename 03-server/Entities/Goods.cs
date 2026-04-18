using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class Goods
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    [Column("Description")]
    public string? Desc { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OldPrice { get; set; }

    [StringLength(255)]
    public string? MainPicture { get; set; }

    public string? Pictures { get; set; }

    public string? Details { get; set; }

    public string? Properties { get; set; }

    public string? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    public int Inventory { get; set; } = 0;

    public int SalesCount { get; set; } = 0;

    public int Status { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<Sku> Skus { get; set; } = new List<Sku>();
    public virtual ICollection<Spec> Specs { get; set; } = new List<Spec>();
}
