using System.Net;
using Domain.DTOs.PaymentDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PaymentService;

public class PaymentService(DataContext context) : IPaymentService
{
    #region GetPaymentsAsync

    public async Task<PagedResponse<List<GetPaymentDto>>> GetPaymentsAsync(PaymentFilter filter)
    {
        try
        {
            var Payments = context.Payments.AsQueryable();

            if (filter.Sum != null)
                Payments = Payments.Where(x => x.Sum == filter.Sum);


            var response = await Payments.Select(x => new GetPaymentDto()
            {
                PaymentStatus = x.PaymentStatus,
                PaymentDate = x.PaymentDate,
                Sum = x.Sum,
                UserId = x.UserId,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord = await Payments.CountAsync();

            return new PagedResponse<List<GetPaymentDto>>(response, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPaymentDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetPaymentByIdAsync

    public async Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int PaymentId)
    {
        try
        {
            var existing = await context.Payments.FirstOrDefaultAsync(x => x.Id == PaymentId);
            if (existing == null) return new Response<GetPaymentDto>(HttpStatusCode.BadRequest, "Not Found");
            var response = new GetPaymentDto()
            {
                PaymentStatus = existing.PaymentStatus,
                PaymentDate = existing.PaymentDate,
                Sum = existing.Sum,
                UserId = existing.UserId,
                Id = existing.Id,
            };
            return new Response<GetPaymentDto>(response);
        }
        catch (Exception e)
        {
            return new Response<GetPaymentDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreatePaymentAsync

    public async Task<Response<string>> CreatePaymentAsync(CreatePaymentDto createPayment)
    {
        try
        {
            var Payment = new Payment()
            {
                PaymentStatus = createPayment.PaymentStatus,
                PaymentDate = createPayment.PaymentDate,
                Sum = createPayment.Sum,
                UserId = createPayment.UserId,
            };
            await context.Payments.AddAsync(Payment);
            await context.SaveChangesAsync();
            return new Response<string>("Successfully created Payment");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdatePaymentAsync

    public async Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto updatePayment)
    {
        try
        {
            var existingPayment = await context.Payments.FirstOrDefaultAsync(x => x.Id == updatePayment.Id);
            if (existingPayment == null) return new Response<string>(HttpStatusCode.BadRequest, "Payment not found");

            existingPayment.PaymentStatus = updatePayment.PaymentStatus;
            existingPayment.Sum = updatePayment.Sum;
            existingPayment.PaymentDate = updatePayment.PaymentDate;
            existingPayment.UserId = updatePayment.UserId;

            await context.SaveChangesAsync();
            return new Response<string>("Successfully updated the Payment");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeletePaymentAsync

    public async Task<Response<bool>> DeletePaymentAsync(int PaymentId)
    {
        try
        {
            var existing = await context.Payments.FirstOrDefaultAsync(x => x.Id == PaymentId);
            if (existing == null) return new Response<bool>(HttpStatusCode.BadRequest, "Not Found");
            context.Payments.Remove(existing);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}