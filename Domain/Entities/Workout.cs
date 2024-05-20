using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Workout
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Duration { get; set; }
    public string Intensity { get; set; } = null!;

    public List<ClassSchedule>? ClassSchedules { get; set; }
}
