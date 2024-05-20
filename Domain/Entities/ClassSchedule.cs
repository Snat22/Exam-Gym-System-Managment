using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ClassSchedule
{
    [Key]
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public int TrainerId { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; }
    public string Location { get; set; } = null!;

    public Workout? Workout { get; set; }
    public Trainer? Trainer { get; set; }

}
