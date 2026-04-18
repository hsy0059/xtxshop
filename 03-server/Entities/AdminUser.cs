using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class AdminUser
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Nickname { get; set; }

    [StringLength(255)]
    public string? Avatar { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    public int Status { get; set; } = 1;

    public int Role { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public DateTime? LastLoginAt { get; set; }
}
