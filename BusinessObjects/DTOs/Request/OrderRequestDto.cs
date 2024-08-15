namespace BusinessObjects.DTOs.Request;

public class OrderRequestDto
{
    public required int MemberId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredDate { get; set; }
    public DateTime ShippedDate { get; set; }
    public string? Freight { get; set; }
}