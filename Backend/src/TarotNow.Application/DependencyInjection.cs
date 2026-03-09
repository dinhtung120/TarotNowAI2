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
        // TODO: Đăng ký MediatR cho CQRS pattern
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        // TODO: Đăng ký FluentValidation
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // TODO: Đăng ký AutoMapper
        // services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
