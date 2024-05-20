using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Trainer
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Specialization { get; set; } = null!;
    public string? Photo { get; set; }

    public List<ClassSchedule>? ClassSchedules { get; set; }


}
