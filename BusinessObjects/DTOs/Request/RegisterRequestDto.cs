namespace BusinessObjects.DTOs.Request;

public class RegisterRequestDto
{
    public string? Email { get; set; }
    public string? CompanyName { get; set; } 
    public string? Password { get; set; }
}