using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Common.Services;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Services;

namespace TarotNow.Application;

// Điểm vào đăng ký dependency cho tầng Application.
public static class DependencyInjection
{
    /// <summary>
    /// Đăng ký toàn bộ service MediatR, validation và domain services của Application.
    /// Luồng xử lý: lần lượt đăng ký MediatR, validator+mapper, rồi các service nghiệp vụ bổ sung.
    /// </summary>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        params Assembly[] additionalAssemblies)
    {
        RegisterMediatR(services, additionalAssemblies);
        RegisterValidation(services);
        RegisterCommandExecutors(services);
        RegisterDomainServices(services);

        return services;
    }

    /// <summary>
    /// Đăng ký MediatR handlers và pipeline behaviors cho assembly hiện tại và assembly bổ sung.
    /// Luồng xử lý: đăng ký assembly chính, thêm assembly phụ, rồi cấu hình behavior theo thứ tự.
    /// </summary>
    private static void RegisterMediatR(IServiceCollection services, Assembly[] additionalAssemblies)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            RegisterAdditionalAssemblies(cfg, additionalAssemblies);
            RegisterPipelineBehaviors(cfg);
        });
    }

    /// <summary>
    /// Đăng ký handlers từ các assembly được truyền thêm.
    /// Luồng xử lý: duyệt từng assembly và gọi RegisterServicesFromAssembly tương ứng.
    /// </summary>
    private static void RegisterAdditionalAssemblies(MediatRServiceConfiguration cfg, IEnumerable<Assembly> additionalAssemblies)
    {
        foreach (var assembly in additionalAssemblies)
        {
            // Bổ sung handler từ module ngoài Application khi cần mở rộng theo plugin/module.
            cfg.RegisterServicesFromAssembly(assembly);
        }
    }

    /// <summary>
    /// Đăng ký chuỗi pipeline behaviors cho MediatR.
    /// Luồng xử lý: thêm logging, performance, validation, caching theo thứ tự thực thi mong muốn.
    /// </summary>
    private static void RegisterPipelineBehaviors(MediatRServiceConfiguration cfg)
    {
        // Logging đặt đầu để ghi nhận toàn bộ request kể cả khi fail ở behavior sau.
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.LoggingBehavior<,>));
        // Performance đo thời gian sau khi logging đã mở context request.
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.PerformanceBehavior<,>));
        // Validation trước caching để chặn dữ liệu sai ngay từ đầu.
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));
        // Transaction bao bọc command để bảo đảm business write và outbox commit cùng ranh giới.
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.CommandTransactionBehavior<,>));
        // Caching ở cuối để cache kết quả đã qua toàn bộ kiểm tra nghiệp vụ.
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.CachingBehavior<,>));
    }

    /// <summary>
    /// Đăng ký FluentValidation cho assembly hiện tại.
    /// Luồng xử lý: scan validator và để mapping theo chuẩn manual mapper tường minh.
    /// </summary>
    private static void RegisterValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Đăng ký các command executor đã tách khỏi command handlers event-only.
    /// Luồng xử lý: quét assembly Application và bind mọi closed interface ICommandExecutionExecutor<,> với implementation tương ứng.
    /// </summary>
    private static void RegisterCommandExecutors(IServiceCollection services)
    {
        var executorInterface = typeof(ICommandExecutionExecutor<,>);
        var executorTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract)
            .Select(type => new
            {
                Implementation = type,
                Contracts = type
                    .GetInterfaces()
                    .Where(contract => contract.IsGenericType
                                       && contract.GetGenericTypeDefinition() == executorInterface)
                    .ToArray()
            })
            .Where(item => item.Contracts.Length > 0)
            .ToArray();

        foreach (var executor in executorTypes)
        {
            foreach (var contract in executor.Contracts)
            {
                services.AddScoped(contract, executor.Implementation);
            }
        }
    }

    /// <summary>
    /// Đăng ký các domain service dùng trong luồng nghiệp vụ Application.
    /// Luồng xử lý: cấu hình vòng đời phù hợp cho settlement service và pricing service.
    /// </summary>
    private static void RegisterDomainServices(IServiceCollection services)
    {
        services.AddScoped<IPipelineBehavior<LoginCommand, LoginResult>, LoginCommandThrottleBehavior>();
        services.AddScoped<IEscrowSettlementService, EscrowSettlementService>();
        services.AddScoped<ICommunityMediaAttachmentService, CommunityMediaAttachmentService>();
        services.AddTransient<FollowupPricingService>();
    }
}
