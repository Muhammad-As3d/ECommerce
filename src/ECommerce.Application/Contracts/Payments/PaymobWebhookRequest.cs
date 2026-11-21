using System.Text.Json.Serialization;

namespace ECommerce.Application.Contracts.Payments;

public sealed class PaymobWebhookRequest
{
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;
    [JsonPropertyName("obj")]
    public PaymobTransaction Obj { get; init; } = null!;
}

public sealed class PaymobTransaction
{
    [JsonPropertyName("id")] public long Id { get; init; }
    [JsonPropertyName("success")] public bool Success { get; init; }
    [JsonPropertyName("pending")] public bool Pending { get; init; }
    [JsonPropertyName("amount_cents")] public long AmountCents { get; init; }
    [JsonPropertyName("currency")] public string Currency { get; init; } = "";
    [JsonPropertyName("created_at")] public string CreatedAt { get; init; } = "";
    [JsonPropertyName("error_occured")] public bool ErrorOccured { get; init; }
    [JsonPropertyName("has_parent_transaction")] public bool HasParentTransaction { get; init; }
    [JsonPropertyName("integration_id")] public long IntegrationId { get; init; }
    [JsonPropertyName("is_3d_secure")] public bool Is3dSecure { get; init; }
    [JsonPropertyName("is_auth")] public bool IsAuth { get; init; }
    [JsonPropertyName("is_capture")] public bool IsCapture { get; init; }
    [JsonPropertyName("is_refunded")] public bool IsRefunded { get; init; }
    [JsonPropertyName("is_standalone_payment")] public bool IsStandalonePayment { get; init; }
    [JsonPropertyName("is_voided")] public bool IsVoided { get; init; }
    [JsonPropertyName("owner")] public long Owner { get; init; }
    [JsonPropertyName("order")] public PaymobOrder Order { get; init; } = null!;
    [JsonPropertyName("source_data")] public PaymobSourceData SourceData { get; init; } = null!;
    [JsonPropertyName("data")] public PaymobData? Data { get; init; }
}

public sealed class PaymobOrder
{
    [JsonPropertyName("id")] public long Id { get; init; }
    [JsonPropertyName("merchant_order_id")] public string? MerchantOrderId { get; init; }
}

public sealed class PaymobSourceData
{
    [JsonPropertyName("pan")] public string? Pan { get; init; }
    [JsonPropertyName("sub_type")] public string? SubType { get; init; }
    [JsonPropertyName("type")] public string? Type { get; init; }
}

public sealed class PaymobData
{
    [JsonPropertyName("message")] public string? Message { get; init; }
}
