namespace ECommerce.Application.Contracts.Products;

public record ProductResponse(
    int Id,
    string Name,
    int ModelYear,
    double ListPrice,
    int CategoryId
);

//public int Id { get; set; }
//public string Name { get; set; } = string.Empty;
//public int ModelYear { get; set; }
//public double ListPrice { get; set; }
//public bool IsActive { get; set; } = true;
//public int CategoryId { get; set; }