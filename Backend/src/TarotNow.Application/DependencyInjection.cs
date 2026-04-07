

using System.Reflection;            
using AutoMapper;
using FluentValidation;             
using MediatR;                      
using Microsoft.Extensions.DependencyInjection; 
using TarotNow.Application.Common.Services;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Services;

namespace TarotNow.Application;


public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        params Assembly[] additionalAssemblies)
    {
        RegisterMediatR(services, additionalAssemblies);
        RegisterValidationAndMapping(services);
        RegisterDomainServices(services);

        return services;
    }

    private static void RegisterMediatR(IServiceCollection services, Assembly[] additionalAssemblies)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            RegisterAdditionalAssemblies(cfg, additionalAssemblies);
            RegisterPipelineBehaviors(cfg);
        });
    }

    private static void RegisterAdditionalAssemblies(MediatRServiceConfiguration cfg, IEnumerable<Assembly> additionalAssemblies)
    {
        foreach (var assembly in additionalAssemblies)
        {
            cfg.RegisterServicesFromAssembly(assembly);
        }
    }

    private static void RegisterPipelineBehaviors(MediatRServiceConfiguration cfg)
    {
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.LoggingBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.PerformanceBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.CachingBehavior<,>));
    }

    private static void RegisterValidationAndMapping(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    private static void RegisterDomainServices(IServiceCollection services)
    {
        services.AddScoped<IEscrowSettlementService, EscrowSettlementService>();
        services.AddTransient<FollowupPricingService>();
    }
}
