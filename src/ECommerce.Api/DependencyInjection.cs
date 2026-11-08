using ECommerce.Api.Authorization;
using Microsoft.AspNetCore.Localization;

namespace ECommerce.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddControllers()
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture("en")
                .AddSupportedCultures("en", "ar", "ar-EG")
                .AddSupportedUICultures("en", "ar", "ar-EG");
            options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
        });

        var assembly = typeof(DependencyInjection).Assembly;

        //Add Mapster
        var mappingConfiguration = TypeAdapterConfig.GlobalSettings;
        mappingConfiguration.Scan(assembly);

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}