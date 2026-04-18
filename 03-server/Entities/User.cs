using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class User
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    [StringLength(20)]
    public string Account { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Nickname { get; set; }

    [StringLength(255)]
    public string? Avatar { get; set; }

    [Required]
    [StringLength(20)]
    public string Mobile { get; set; } = string.Empty;

    [StringLength(10)]
    public string? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    [StringLength(100)]
    public string? FullLocation { get; set; }

    [StringLength(50)]
    public string? Profession { get; set; }

    [StringLength(20)]
    public string? ProvinceCode { get; set; }

    [StringLength(20)]
    public string? CityCode { get; set; }

    [StringLength(20)]
    public string? CountyCode { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
