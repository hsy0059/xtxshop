using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class Banner
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    [StringLength(255)]
    public string ImgUrl { get; set; } = string.Empty;

    [StringLength(255)]
    public string? HrefUrl { get; set; }

    public int Type { get; set; } = 1;

    public int Sort { get; set; } = 0;

    public int Status { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
