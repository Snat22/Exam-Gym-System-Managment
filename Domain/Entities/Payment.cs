using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Payment
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Sum { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentStatus { get; set; } = null!;

    public User? User { get; set; }
}
