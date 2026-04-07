

using System.Reflection;            
using AutoMapper;
using FluentValidation;             
using MediatR;                      
using Microsoft.Extensions.DependencyInjection; 
using TarotNow.Application.Interfaces;
using TarotNow.Application.Services;
using TarotNow.Domain.Services;

namespace TarotNow.Application;


public static class DependencyInjection
{
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, params Assembly[] additionalAssemblies)
    {
        
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            foreach (var assembly in additionalAssemblies)
            {
                cfg.RegisterServicesFromAssembly(assembly);
            }
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.PerformanceBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.CachingBehavior<,>));
        });
        
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IEscrowSettlementService, EscrowSettlementService>();

        
        services.AddTransient<FollowupPricingService>();

        return services; 
    }
}
