namespace ECommerce.Application.Abstractions.Constants;

public static class Permissions
{
    public static string Type => "permissions";

    public const string GetAddress = "address:read";
    public const string UpdateAddress = "address:update";
    public const string DeleteAddress = "address:delete";

    public const string GetCarts = "cart:read";
    public const string AddToCart = "cart:add";
    public const string RemoveFromCart = "cart:remove";
    public const string UpdateCart = "cart:update";
    public const string ClearCart = "cart:clear";

    public const string GetOrders = "order:read";
    public const string GetAllOrders = "order:read all";
    public const string GetUserOrders = "order:read user";
    public const string CheckoutOrders = "order:checkout";
    public const string CancelOrders = "order:cancel";
    public const string ProcessOrders = "order:process";
    public const string ShipOrders = "order:ship";
    public const string DeliverOrders = "order:deliver";
    public const string StatusHistoryOrders = "order:status-history";
    public const string UpdateOrder = "order:update";
    public const string DeleteOrder = "order:delete";

    public const string GetCategories = "category:read";
    public const string GetAllCategories = "category:read all";
    public const string GetCategoryProducts = "category:read products";
    public const string UpdateCategories = "category:update";
    public const string AddCategories = "category:add";
    public const string ToggleCategories = "category:toggle";

    public const string GetProducts = "product:read";
    public const string AddProduct = "product:add";
    public const string UpdateProducts = "product:update";
    public const string ToggleProducts = "product:toggle";
    public const string AddProductImages = "product:add images";
    public const string RemoveProductImages = "product:remove images";

    //Users
    public const string UsersGetAll = "user:getAll";
    public const string UsersGet = "user:get";
    public const string UsersAdd = "user:Add";
    public const string UsersUpdate = "user:update";
    public const string UsersDisable = "user:disable";
    public const string UsersUnlock = "user:unlock";

    public static IList<string?> GetAllPermissions() =>
        [.. typeof(Permissions).GetFields().Select(f => f.GetValue(f) as string)];
}