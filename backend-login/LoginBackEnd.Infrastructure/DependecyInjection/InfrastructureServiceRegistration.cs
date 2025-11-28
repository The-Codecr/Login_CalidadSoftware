using LoginBackEnd.Application.Auth;
using LoginBackEnd.Domain.Users;
using LoginBackEnd.Infrastructure.Auth;
using LoginBackEnd.Infrastructure.Auth.Jwt;
using LoginBackEnd.Infrastructure.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoginBackEnd.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Register JWT
        services.AddJwtAuthentication(config);

        // Repositorios
        services.AddScoped<IUserRepository, UserRepository>();

        // Servicios
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
