namespace BusinessObjects.DTOs.Response;

public class OrderResponseDto
{
    public required string OrderId { get; set; }
    public required int MemberId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredDate { get; set; }
    public DateTime ShippedDate { get; set; }
    public string? Freight { get; set; }
}