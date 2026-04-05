using ECommerce.Application.Abstractions.Results;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands.Requests;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result>;
