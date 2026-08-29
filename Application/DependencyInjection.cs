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
            services.AddAutoMapper(_ => { }, typeof(DependencyInjection).Assembly);
            services.AddScoped<IUserResolverService, UserResolverService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenRefreshService, TokenRefreshService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IUserSkillRelationService, UserSkillRelationService>();
            services.AddScoped<IEmailOutboxService, EmailOutboxService>();
            services.AddScoped<IPendingEmailConfirmationService, PendingEmailConfirmationService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }
    }
}
