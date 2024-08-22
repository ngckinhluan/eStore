using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities;

public class Category 
{
    public required int CategoryId { get; set; }
    [MaxLength(255)]
    public string? CategoryName { get; set; }
    // Navigation properties
    public IEnumerable<Product>? Products { get; set; }
}