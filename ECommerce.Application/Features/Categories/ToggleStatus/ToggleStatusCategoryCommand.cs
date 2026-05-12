using ECommerce.Domain.Abstractions;
using MediatR;

namespace ECommerce.Application.Features.Categories.ToggleStatus;

public record ToggleStatusCategoryCommand(int Id) : IRequest<Result>;