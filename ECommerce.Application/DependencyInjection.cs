using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddMediatR(op => op.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
