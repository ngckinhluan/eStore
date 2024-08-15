namespace BusinessObjects.DTOs.Response;

public class MemberResponseDto
{
    public required int MemberId { get; set; }
    public string? Email { get; set; }
    public string? CompanyName { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Password { get; set; }
}