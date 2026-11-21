using System.Text.Json.Serialization;

namespace ECommerce.Infrastructure.Clients;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(PaymobIntentionBody))]
[JsonSerializable(typeof(PaymobIntentionResponse))]
public sealed partial class PaymobJsonContext : JsonSerializerContext;
