using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities;

public class Order
{
    public required int OrderId { get; set; }
    public required int MemberId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredDate { get; set; }
    public DateTime ShippedDate { get; set; }
    [MaxLength(255)]
    public string? Freight { get; set; }
    
    // Navigation properties
    public Member? Member { get; set; }
    public IEnumerable<OrderDetail>? OrderDetails { get; set; }
}