using ECommerce.Api.Middleware;
using ECommerce.Application;
using ECommerce.Infrastructure.DependencyInjection;

namespace ECommerce.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApi();

        services
            .AddApplicationDependencies()
            .AddInfrastructureDependencies(configuration);


        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddAutoMapper(cfg => { }, typeof(DependencyInjection).Assembly);

        return services;
    }
}
