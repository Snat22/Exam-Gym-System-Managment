using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.TrainerDto;

public class GetTrainerDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Specialization { get; set; }
    public string? Photo { get; set; }
}
