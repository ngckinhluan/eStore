using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities;

public class OrderDetail
{
    public required int OrderId { get; set; }
    public required int ProductId { get; set; }
    public int Quantity { get; set; }
    public float UnitPrice { get; set; }
    
    // Navigation properties
    public Order? Order { get; set; }
    public Product? Product { get; set; }
}