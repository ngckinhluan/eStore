namespace BusinessObjects.DTOs.Response;

public class OrderDetailResponseDto
{
    public required int OrderId { get; set; }
    public required int ProductId { get; set; }
    public int Quantity { get; set; }
    public float UnitPrice { get; set; }
}