using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtxServer.Entities;

public class Order
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    public int OrderState { get; set; } = 1;

    [StringLength(50)]
    public string? ReceiverContact { get; set; }

    [StringLength(20)]
    public string? ReceiverMobile { get; set; }

    [StringLength(200)]
    public string? ReceiverAddress { get; set; }

    public int DeliveryTimeType { get; set; } = 1;

    [StringLength(500)]
    public string? BuyerMessage { get; set; }

    public int PayType { get; set; } = 1;

    public int PayChannel { get; set; } = 2;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalMoney { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PostFee { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PayMoney { get; set; }

    public DateTime? PayTime { get; set; }

    public DateTime? ShipTime { get; set; }

    public DateTime? ReceiveTime { get; set; }

    public DateTime? CancelTime { get; set; }

    [StringLength(200)]
    public string? CancelReason { get; set; }

    public DateTime CreateTime { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int Countdown { get; set; } = 1800;

    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
