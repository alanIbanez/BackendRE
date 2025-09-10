using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<AuthService>();
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}