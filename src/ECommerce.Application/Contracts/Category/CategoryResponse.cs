namespace ECommerce.Application.Contracts.Category;

public record CategoryResponse(
    Guid Id,
    string Name,
    string Description
);
