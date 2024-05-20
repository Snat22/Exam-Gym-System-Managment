using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Membership
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Type { get; set; } = null!;
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Photo { get; set; }

    public User? User { get; set; }

}
