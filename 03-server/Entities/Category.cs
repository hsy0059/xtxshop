using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class Category
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Icon { get; set; }

    [StringLength(255)]
    public string? Picture { get; set; }

    public int Sort { get; set; } = 0;

    public int Level { get; set; } = 1;

    public string? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public virtual Category? Parent { get; set; }

    public virtual ICollection<Category> Children { get; set; } = new List<Category>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
