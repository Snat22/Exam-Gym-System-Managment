namespace Domain.DTOs.PaymentDto;

public class GetPaymentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Sum { get; set; }
    public DateTime PaymentDate { get; set; }
    public required string PaymentStatus { get; set; }
}
