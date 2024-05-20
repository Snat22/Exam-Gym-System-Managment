using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.TrainerDto;

public class CreateTrainerDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Specialization { get; set; }
    public IFormFile? Photo { get; set; }
}
