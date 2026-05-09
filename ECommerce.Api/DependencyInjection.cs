using ECommerce.Api.Middleware;
using ECommerce.Application;
using ECommerce.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApi();

        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(allowedOrigins!)
            )
        );


        services.AddApplicationDependencies();
        services.AddInfrastructureDependencies(configuration);


        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        //services.AddAutoMapper(cfg => { }, typeof(DependencyInjection).Assembly);



        return services;
    }
}
