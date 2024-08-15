using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities;

public class Member
{
    public required int MemberId { get; set; }
    [MaxLength(255)]
    public string? Email { get; set; }
    [MaxLength(255)]
    public string? CompanyName { get; set; }
    [MaxLength(255)]
    public string? City { get; set; }
    [MaxLength(255)]
    public string? Country { get; set; }
    [MaxLength(50)]
    public string? Password { get; set; }
    
    // Navigation properties
    public IEnumerable<Order>? Orders { get; set; }
    
}