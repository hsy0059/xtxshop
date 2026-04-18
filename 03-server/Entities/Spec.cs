using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class Spec
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    public string GoodsId { get; set; } = string.Empty;

    [ForeignKey("GoodsId")]
    public virtual Goods Goods { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Column("Values")]
    public string? Values { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
