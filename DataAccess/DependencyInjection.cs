using DataAccess.DbContexts;
using Application.Common.Persistence;
using DataAccess.Services;
using Application.Common.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = PostgreSqlConnectionString.Resolve(configuration);

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    b => b
                        .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                        .CommandTimeout(30)
                ).UsePortfolioQuerySafety());
            services.AddScoped<IAppDbContext, AppDbContext>();
            // PasswordHasher<TUser> is stateless after construction. Keeping one
            // instance also ensures the expensive dummy-account hash is generated
            // once per application lifetime rather than once per request.
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMaintenanceCleanupService, MaintenanceCleanupService>();
            services.AddScoped<IDatabaseReadinessService, DatabaseReadinessService>();
            services.AddScoped<IContactSubmissionGuard, ContactSubmissionGuard>();
            services.AddScoped<IEmailConfirmationLock, EmailConfirmationLock>();
            services.AddSingleton<IPersistenceExceptionClassifier, PersistenceExceptionClassifier>();

            return services;
        }
    }
}
