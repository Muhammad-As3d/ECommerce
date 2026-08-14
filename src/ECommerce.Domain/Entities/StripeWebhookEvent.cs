using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public sealed class StripeWebhookEvent : BaseEntity
{
    public string StripeEventId { get; private set; } = string.Empty;
    public string EventType { get; private set; } = string.Empty;
    public string? Payload { get; private set; }

    public DateTimeOffset ReceivedAt { get; private set; }
    public DateTimeOffset? ProcessedAt { get; private set; }
    public string? ProcessingError { get; private set; }

    private StripeWebhookEvent() { }

    public static StripeWebhookEvent Create(string stripeEventId, string eventType, string? payload)
    {
        return new StripeWebhookEvent
        {
            StripeEventId = stripeEventId,
            EventType = eventType,
            Payload = payload,
            ReceivedAt = DateTimeOffset.UtcNow
        };
    }

    public void MarkProcessed()
    {
        ProcessedAt = DateTimeOffset.UtcNow;
        ProcessingError = null;
    }

    public void MarkFailed(string error)
    {
        ProcessingError = error;
    }
}