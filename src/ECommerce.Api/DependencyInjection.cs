using ECommerce.Api.Exceptions;
using ECommerce.Application;
using ECommerce.Infrastructure;
using System.Text.Json.Serialization;

namespace ECommerce.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddControllers()
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.Converters.Add(
                     new JsonStringEnumConverter());
             });

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

        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        var assembly = typeof(DependencyInjection).Assembly;

        //Add Mapster
        var mappingConfiguration = TypeAdapterConfig.GlobalSettings;
        mappingConfiguration.Scan(assembly);

        return services;
    }
}
