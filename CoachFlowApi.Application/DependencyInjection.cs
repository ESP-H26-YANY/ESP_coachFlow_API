using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation; // Nécessaire pour AddValidatorsFromAssembly

using CoachFlowApi.Application.UseCases.User;
using CoachFlowApi.Application.UseCases.User.Interfaces;
using CoachFlowApi.Application.UseCases.Guide;
using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Application.UseCases.Library;
using CoachFlowApi.Application.UseCases.Library.Interfaces;

namespace CoachFlowApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<ICreateGuideUseCase, CreateGuideUseCase>();
        services.AddScoped<IDeleteGuideUseCase, DeleteGuideUseCase>();
        services.AddScoped<IGetAllGuidesUseCase, GetAllGuidesUseCase>();
        services.AddScoped<IGetGuidesByUserUseCase, GetGuidesByUserUseCase>();
        services.AddScoped<IGetGuideByIdUseCase, GetGuideByIdUseCase>();
        services.AddScoped<IUpdateGuideUseCase, UpdateGuideUseCase>();
        services.AddScoped<IAddToLibraryUseCase, AddToLibraryUseCase>();
        services.AddScoped<IRemoveFromLibraryUseCase, RemoveFromLibraryUseCase>();
        services.AddScoped<IGetMyLibraryUseCase, GetMyLibraryUseCase>();
        services.AddScoped<IPurchaseGuideUseCase, PurchaseGuideUseCase>();
        services.AddScoped<IGetPurchasedGuidesUseCase, GetPurchasedGuidesUseCase>();
        services.AddScoped<IDownloadGuideUseCase, DownloadGuideUseCase>();

        return services;
    }
}