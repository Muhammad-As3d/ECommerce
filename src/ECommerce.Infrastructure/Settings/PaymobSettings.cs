namespace ECommerce.Infrastructure.Settings;

public sealed class PaymobSettings
{
    public const string SectionName = "Paymob";

    [Required] public string BaseUrl { get; init; } = string.Empty;
    [Required] public string SecretKey { get; init; } = string.Empty;
    [Required] public string PublicKey { get; init; } = string.Empty;
    [Required] public string HmacSecret { get; init; } = string.Empty;
    [Range(1, long.MaxValue)] public long CardIntegrationId { get; init; }
    [Required] public string NotificationUrl { get; init; } = string.Empty;
    [Required] public string RedirectionUrl { get; init; } = string.Empty;
}
