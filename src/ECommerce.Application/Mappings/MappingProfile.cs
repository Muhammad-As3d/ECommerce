namespace ECommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Category
        CreateMap<Category, CategoryResponse>();

        CreateMap<Category, CategoryProductsResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        #endregion

        //Product Mappings
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.ImageURLs,
            opt => opt.MapFrom(src => src.ProductImages.Select(x => x.ImageUrl).ToList()));

        CreateMap<Product, ProductImagesResponse>()
            .ForMember(dest => dest.ImagesUrls,
            opt => opt.MapFrom(src => src.ProductImages.Select(x => x.ImageUrl).ToList()));

        CreateMap<ProductImage, ProductImageResponse>()
            .ConstructUsing(src => new ProductImageResponse(src.Id, src.ImageUrl));

        #region Cart
        CreateMap<CartItem, CartItemResponse>()
            .ConstructUsing(src =>
            new CartItemResponse(src.Id,
            src.ProductId,
            src.Product.Name,
            src.Product.ProductImages.Select(x => x.ImageUrl).FirstOrDefault() ?? string.Empty,
            src.UnitPriceSnapshot,
            src.Quantity,
            src.Quantity * src.UnitPriceSnapshot,
            src.Quantity < src.Product.Stock));

        CreateMap<Cart, CartResponse>()
            .ConstructUsing(src => new CartResponse(src.Id, src.CartItems!.Count()));

        #endregion

        #region Address

        CreateMap<Address, AddressResponse>();
        #endregion

        #region Order

        CreateMap<ShippingAddressSnapshot, ShippingAddressResponse>()
            .ConstructUsing(address => new ShippingAddressResponse(
                address.FullName,
                address.Street,
                address.City,
                address.Governorate,
                address.Country,
                address.PostalCode,
                address.PhoneNumber));

        CreateMap<Order, OrderUserResponse>()
            .ConstructUsing(src => new OrderUserResponse(
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

        CreateMap<OrderItem, OrderItemResponse>()
            .ConstructUsing(item => new OrderItemResponse(
                item.Id,
                item.ProductNameSnapshot,
                item.UnitPriceSnapshot,
                item.Quantity,
                item.Quantity * item.UnitPriceSnapshot
            ));

        CreateMap<Order, OrderDetailsResponse>()
            .ConstructUsing(src => new OrderDetailsResponse(
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

        CreateMap<Order, OrderAdminResponse>()
            .ConstructUsing(src => new OrderAdminResponse(
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

        CreateMap<CartItem, CartItemCheckoutInfo>();
        CreateMap<Address, ShippingAddressInfo>();

        #endregion
    }
}
