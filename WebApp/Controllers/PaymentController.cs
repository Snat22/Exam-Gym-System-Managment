using Domain.DTOs.PaymentDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController(IPaymentService PaymentService) : ControllerBase
{
    [HttpGet]
    public async Task<Response<List<GetPaymentDto>>> GetPaymentsAsync([FromQuery] PaymentFilter PaymentFilter)
        => await PaymentService.GetPaymentsAsync(PaymentFilter);

    [HttpGet("{PaymentId:int}")]
    public async Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int PaymentId)
        => await PaymentService.GetPaymentByIdAsync(PaymentId);

    [HttpPost("create")]
    public async Task<Response<string>> CreatePaymentAsync([FromForm] CreatePaymentDto Payment)
        => await PaymentService.CreatePaymentAsync(Payment);


    [HttpPut("update")]
    public async Task<Response<string>> UpdatePaymentAsync([FromForm] UpdatePaymentDto Payment)
        => await PaymentService.UpdatePaymentAsync(Payment);

    [HttpDelete("{PaymentId:int}")]
    public async Task<Response<bool>> DeletePaymentAsync(int PaymentId)
        => await PaymentService.DeletePaymentAsync(PaymentId);
}
