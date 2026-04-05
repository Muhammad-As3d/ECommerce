using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands.Requests;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result>;
