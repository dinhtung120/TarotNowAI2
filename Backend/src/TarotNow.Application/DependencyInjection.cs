using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TarotNow.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Đăng ký tất cả các Application Services vào DI container.
    /// Giúp Program.cs gọn gàng hơn (Clean Architecture).
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Đăng ký MediatR cho CQRS pattern
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            // Thêm Pipeline Behavior
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));
        });
        
        // Đăng ký FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
