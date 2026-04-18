using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class Address
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Receiver { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Contact { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ProvinceCode { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string CityCode { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string CountyCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FullLocation { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string AddressDetail { get; set; } = string.Empty;

    public int IsDefault { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
