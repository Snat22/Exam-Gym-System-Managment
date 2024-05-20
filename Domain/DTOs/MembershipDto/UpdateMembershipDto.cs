using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.MembershipDto;

public class UpdateMembershipDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Type { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IFormFile? Photo { get; set; }
}
