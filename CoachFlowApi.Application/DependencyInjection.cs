using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation; // Nécessaire pour AddValidatorsFromAssembly

// Nécessaires pour trouver tes UseCases
using CoachFlowApi.Application.UseCases.User;
using CoachFlowApi.Application.UseCases.User.Interfaces;

namespace CoachFlowApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Enregistre automatiquement tous les validateurs (LoginValidator, etc.)
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Enregistre les UseCases
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        
        return services;
    }
}