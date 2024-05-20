namespace Domain.DTOs.UserDto;

public class CreateUserDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateTime RegisterDate { get; set; }
    public required string Role { get; set; }
}
