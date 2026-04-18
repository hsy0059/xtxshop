using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class HotRecommend
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Alt { get; set; }

    [StringLength(255)]
    public string? Target { get; set; }

    public string? Pictures { get; set; }

    [StringLength(50)]
    public string Type { get; set; } = string.Empty;

    public int Sort { get; set; } = 0;

    public int Status { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
