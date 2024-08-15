namespace BusinessObjects.DTOs.Response;

public class ProductResponseDto
{
    public required int ProductId { get; set; }
    public required int CategoryId { get; set; }
    public string? ProductName { get; set; }
    public float Weight { get; set; }
    public float UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
}