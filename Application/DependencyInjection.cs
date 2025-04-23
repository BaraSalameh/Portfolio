using Application.Common.Services.Interface;
using Application.Common.Services.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUserResolverService, UserResolverService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddHostedService<RefreshTokenCleanupService>();
            return services;
        }
    }
}