namespace ECommerce.Application.Features.Payments;

public static class PaymentExtensions
{
    public static long ToCents(this decimal amount) =>
        (long)Math.Round(amount * 100, MidpointRounding.AwayFromZero);
}
