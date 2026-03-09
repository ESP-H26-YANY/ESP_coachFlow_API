using CoachFlowApi.Domain.Interfaces.Repositories; 
using CoachFlowApi.Infrastructure.Data;
using CoachFlowApi.Infrastructure.Repositories;
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

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

         services.AddScoped<IUserRepository, UserRepository>();
         services.AddScoped<IGuideRepository, GuideRepository>();
         services.AddScoped<ICoachRepository, CoachRepository>();
         services.AddScoped<ILibraryRepository, LibraryRepository>();


        return services;
    }
}