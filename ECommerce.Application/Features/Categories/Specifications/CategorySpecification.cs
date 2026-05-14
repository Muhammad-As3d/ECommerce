using ECommerce.Domain.Specifications;

namespace ECommerce.Application.Features.Categories.Specifications;

public class CategorySpecification : Specification<Category>
{
    public CategorySpecification(string? search, int? id, bool isDescending)
    {
        Predicate = x => (string.IsNullOrEmpty(search) || x.Name.Contains(search)) && (!id.HasValue || x.Id == id);

        SortingBy(x => x.Name);

        if (isDescending)
            SortingByDescending(x => x.Name);
    }
}
