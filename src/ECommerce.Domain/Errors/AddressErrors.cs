using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class AddressErrors
{
    public static Error AddressNotFound =>
       Error.NotFound("Address.AddressNotFound", "this user haven't any addresses.");
}
