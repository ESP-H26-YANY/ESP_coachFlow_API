using CoachFlowApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoachFlowApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Configuration pour MySQL (Pomelo) au lieu de SQL Server
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<ICoachRepository, CoachRepository>();
        // services.AddScoped<IGuideRepository, GuideRepository>();
        // services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        // services.AddScoped<IPurchaseRepository, PurchaseRepository>();

        return services;
    }
}