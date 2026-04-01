using ECommerce.Application.DependencyInjection;
using ECommerce.Infrastructure.DependencyInjection;

namespace ECommerce.Api.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApi();

        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);


        return services;
    }
}
