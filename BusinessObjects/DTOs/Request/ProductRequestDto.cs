namespace BusinessObjects.DTOs.Request;

public class ProductRequestDto
{ 
    public string? ProductName { get; set; }
    public int CategoryId { get; set; }
    public float Weight { get; set; }
    public float UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
}