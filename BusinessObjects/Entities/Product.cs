using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities;

public class Product
{
    public required int ProductId { get; set; }
    public required int CategoryId { get; set; }
    [MaxLength(255)]
    public string? ProductName { get; set; }
    public float Weight { get; set; }
    public float UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
    
    // Navigation properties
    public Category? Category { get; set; }
    public IEnumerable<OrderDetail>? OrderDetails { get; set; }
}