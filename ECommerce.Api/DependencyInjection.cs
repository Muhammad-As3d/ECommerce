using ECommerce.Application;
using ECommerce.Infrastructure.DependencyInjection;
using FluentValidation;

namespace ECommerce.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApi();

        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);


        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddAutoMapper(cfg => { }, typeof(DependencyInjection).Assembly);

        return services;
    }
}
