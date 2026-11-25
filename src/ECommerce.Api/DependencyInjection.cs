using ECommerce.Api.Authorization;
using ECommerce.Api.Services;
using ECommerce.Application.Interfaces.Services;
using System.Net;

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
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                var problem = ctx.ProblemDetails;
                var httpContext = ctx.HttpContext;
                var request = httpContext.Request;
                var status = problem.Status ?? httpContext.Response.StatusCode;

                problem.Instance ??= $"{request.Method} {request.Path}"; // RFC 9457

                problem.Extensions.TryAdd("code", problem is ValidationProblemDetails
                    ? "General.Validation"
                    : $"General.{(HttpStatusCode)status}");

                var localizer = httpContext.RequestServices.GetRequiredService<IStringLocalizer>();
                var title = localizer[problem is ValidationProblemDetails ? "Validation.Title" : $"ProblemTitle.{status}"];
                if (!title.ResourceNotFound)
                    problem.Title = title.Value;
            };
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            foreach (var mapping in options.ClientErrorMapping.Values)
                mapping.Link = mapping.Link?.Replace(
                    "https://tools.ietf.org/html/rfc9110", "https://www.rfc-editor.org/rfc/rfc9110");
        });

        var assembly = typeof(DependencyInjection).Assembly;

        //Add Mapster
        var mappingConfiguration = TypeAdapterConfig.GlobalSettings;
        mappingConfiguration.Scan(assembly);

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.AddSignalR();
        services.AddScoped<IRealTimeNotifier, SignalRNotifier>();

        return services;
    }
}