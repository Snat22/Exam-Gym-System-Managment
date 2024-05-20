namespace Domain.DTOs.PaymentDto;

public class CreatePaymentDto
{
    public int UserId { get; set; }
    public decimal Sum { get; set; }
    public DateTime PaymentDate { get; set; }
    public required string PaymentStatus { get; set; }
}
