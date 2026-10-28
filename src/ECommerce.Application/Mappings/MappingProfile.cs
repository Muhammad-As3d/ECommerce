namespace ECommerce.Application.Mappings;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.MapToConstructor(true);

        #region Category
        config.NewConfig<Category, CategoryResponse>();

        config.NewConfig<Category, CategoryProductsResponse>()
            .Map(dest => dest.Products, src => src.Products);

        #endregion

        //Product Mappings
        config.NewConfig<Product, ProductResponse>()
            .Map(dest => dest.ImageURLs,
                src => src.ProductImages.Select(x => ToPublicImageUrl(x.ImageUrl)).ToList());

        config.NewConfig<Product, ProductImagesResponse>()
            .Map(dest => dest.ImagesUrls,
                src => src.ProductImages.Select(x => ToPublicImageUrl(x.ImageUrl)).ToList());

        config.NewConfig<ProductImage, ProductImageResponse>()
            .MapWith(src => new ProductImageResponse(src.Id, ToPublicImageUrl(src.ImageUrl)));

        #region Cart
        config.NewConfig<CartItem, CartItemResponse>()
            .MapWith(src =>
            new CartItemResponse(src.Id,
            src.ProductId,
            src.Product.Name,
            ToPublicImageUrl(src.Product.ProductImages.Select(x => x.ImageUrl).FirstOrDefault()),
            src.UnitPriceSnapshot,
            src.Quantity,
            src.Quantity * src.UnitPriceSnapshot,
            src.Quantity < src.Product.Stock));

        config.NewConfig<Cart, CartResponse>()
            .MapWith(src => new CartResponse(src.Id, src.CartItems!.Count()));

        #endregion

        #region Address

        config.NewConfig<Address, AddressResponse>();
        #endregion

        #region Order

        config.NewConfig<ShippingAddressSnapshot, ShippingAddressResponse>()
            .MapWith(address => new ShippingAddressResponse(
                address.FullName,
                address.Street,
                address.City,
                address.Governorate,
                address.Country,
                address.PostalCode,
                address.PhoneNumber));

        config.NewConfig<Order, OrderUserResponse>()
            .MapWith(src => new OrderUserResponse(
                src.Id,
                src.OrderNumber,
                src.Status.ToString(),
                src.CreatedOn,
                src.Items.Count(),
                src.EstimatedDeliveryFrom,
                src.EstimatedDeliveryTo,
                src.SubTotal,
                src.DiscountAmount,
                src.ShippingFee,
                src.TaxAmount,
                src.TotalAmount
            ));

        config.NewConfig<OrderItem, OrderItemResponse>()
            .MapWith(item => new OrderItemResponse(
                item.Id,
                item.ProductNameSnapshot,
                item.UnitPriceSnapshot,
                item.Quantity,
                item.Quantity * item.UnitPriceSnapshot
            ));

        config.NewConfig<Order, OrderDetailsResponse>()
            .MapWith(src => new OrderDetailsResponse(
                src.Id,
                src.OrderNumber,
                src.Status.ToString(),
                src.CreatedOn,
                src.PaymentMethod.ToString(),
                src.Currency,
                src.EstimatedDeliveryFrom,
                src.EstimatedDeliveryTo,
                src.SubTotal,
                src.DiscountAmount,
                src.ShippingFee,
                src.TaxAmount,
                src.TotalAmount,
                new ShippingAddressResponse(
                    src.ShippingAddress.FullName,
                    src.ShippingAddress.Street,
                    src.ShippingAddress.City,
                    src.ShippingAddress.Governorate,
                    src.ShippingAddress.Country,
                    src.ShippingAddress.PostalCode,
                    src.ShippingAddress.PhoneNumber
                ),
                src.Items.Select(item => new OrderItemResponse(
                    item.Id,
                    item.ProductNameSnapshot,
                    item.UnitPriceSnapshot,
                    item.Quantity,
                    item.Quantity * item.UnitPriceSnapshot
                )).ToList()
            ));

        config.NewConfig<Order, OrderAdminResponse>()
            .MapWith(src => new OrderAdminResponse(
                src.Id,
                src.OrderNumber,
                src.ShippingAddress.FullName,
                src.Status.ToString(),
                src.Payments.OrderByDescending(x => x.CreatedOn)
                    .Select(x => x.Status.ToString())
                    .FirstOrDefault() ?? "Pending",
                src.CreatedOn,
                src.PaymentMethod.ToString(),
                src.SubTotal,
                src.TotalAmount,
                src.UserId
            ));

        config.NewConfig<CartItem, CartItemCheckoutInfo>();
        config.NewConfig<Address, ShippingAddressInfo>();

        #endregion
    }

    private static string ToPublicImageUrl(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return string.Empty;

        var normalizedPath = imagePath.Replace('\\', '/');
        var fileName = normalizedPath[(normalizedPath.LastIndexOf('/') + 1)..];

        return $"/Images/{fileName}";
    }
}
