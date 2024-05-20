namespace Domain.DTOs.ClassScheduleDto;

public class UpdateClassScheduleDto
{
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public int TrainerId { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; }
    public required string Location { get; set; }
}
