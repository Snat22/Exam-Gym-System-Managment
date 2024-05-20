namespace Domain.DTOs.ClassScheduleDto;

public class CreateClassScheduleDto
{
    public int WorkoutId { get; set; }
    public int TrainerId { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; }
    public required string Location { get; set; }
}
