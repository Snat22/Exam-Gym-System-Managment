using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;


public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string HashPassword { get; set; } = null!;
    public DateTime RegisterDate { get; set; }
    public string Role { get; set; } = null!;

    public List<Payment>? Payments { get; set; }
    public List<Membership>? Memberships { get; set; }

}
