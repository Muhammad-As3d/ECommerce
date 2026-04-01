namespace ECommerce.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public double ListPrice { get; set; }
    public double Discount { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    public int OrderId { get; set; }
    public int ProductId { get; set; }

    public Order Order { get; set; } = default!;
    public Product Product { get; set; } = default!;
}
