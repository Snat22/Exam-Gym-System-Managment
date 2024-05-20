namespace Domain.Filters;

public class PaymentFilter : PaginationFilter
{
    public decimal? Sum { get; set; }

}
