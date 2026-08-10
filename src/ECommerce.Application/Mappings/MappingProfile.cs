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

        CreateMap<Product, CartProductResponse>()
            .ConstructUsing(src => new CartProductResponse(src.Stock, src.Price));

        CreateMap<Cart, CartResponse>()
            .ConstructUsing(src => new CartResponse(src.Id, src.CartItems!.Count()));

        #endregion

        #region Address

        CreateMap<Address, AddressResponse>();
        #endregion

        #region Order

        CreateMap<Order, OrderUserResponse>()
            .ConstructUsing(src => new OrderUserResponse(
                src.Id,
                src.OrderNumber,
                src.Status.ToString(),
                src.CreatedOn,
                src.Items.Count(),
                src.WithinDays,
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
                src.WithinDays,
                src.SubTotal,
                src.DiscountAmount,
                src.ShippingFee,
                src.TaxAmount,
                src.TotalAmount,
                new AddressResponse(
                    src.ShippingAddress.Id,
                    src.ShippingAddress.Street,
                    src.ShippingAddress.City,
                    src.ShippingAddress.Governorate,
                    src.ShippingAddress.Country,
                    src.ShippingAddress.PostalCode,
                    src.ShippingAddress.PhoneNumber,
                    src.ShippingAddress.IsDefault
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
                src.Status.ToString(),
                src.Payment.Status.ToString() ?? "Pending",
                src.CreatedOn,
                src.WithinDays,
                src.SubTotal,
                src.TotalAmount,
                src.UserId
            ));

        #endregion
    }
}