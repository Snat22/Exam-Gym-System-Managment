
namespace Domain.DTOs.UserDto;

public class GetUserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string HashPassword { get; set; }
    public DateTime RegisterDate { get; set; }
    public required string Role { get; set; }

}
