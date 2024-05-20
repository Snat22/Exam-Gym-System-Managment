namespace Domain.DTOs.WorkoutDto;

public class GetWorkoutDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public required string Intensity { get; set; }
}
