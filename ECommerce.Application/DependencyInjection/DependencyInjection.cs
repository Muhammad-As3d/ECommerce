using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        services.AddMediatR(op => op.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
