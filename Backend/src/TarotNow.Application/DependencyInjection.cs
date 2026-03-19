/*
 * ===================================================================
 * FILE: DependencyInjection.cs
 * NAMESPACE: TarotNow.Application
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Đăng ký tất cả SERVICE của Application layer vào DI Container.
 *   Được gọi từ Program.cs: builder.Services.AddApplicationServices()
 *
 * DEPENDENCY INJECTION (DI) LÀ GÌ?
 *   Thay vì class tự tạo dependency:
 *     var mediator = new Mediator();  ← KHÔNG NÊN (hard-coded, khó test)
 *   
 *   DI Container tạo và TIÊM vào (inject) qua constructor:
 *     public MyHandler(IMediator mediator)  ← TỐT (linh hoạt, dễ test)
 *   
 *   File này ĐĂNG KÝ vào container: "Khi ai cần IMediator → tạo Mediator"
 *   Sau đó, bất cứ class nào "xin" IMediator qua constructor → DI tự cung cấp.
 *
 * EXTENSION METHOD:
 *   "this IServiceCollection services" biến hàm thành extension method.
 *   Cho phép gọi: services.AddApplicationServices() (như method của services).
 *   Thay vì: DependencyInjection.AddApplicationServices(services) (dài dòng).
 *
 * TẠI SAO MỖI LAYER CÓ FILE DI RIÊNG?
 *   Clean Architecture: mỗi layer quản lý đăng ký DI của mình.
 *   - Application → DependencyInjection.cs (MediatR, Validation)
 *   - Infrastructure → DependencyInjection.cs (EF Core, MongoDB, Redis, JWT)
 *   Program.cs chỉ gọi 2 dòng, không biết chi tiết bên trong.
 * ===================================================================
 */

using System.Reflection;            // Assembly.GetExecutingAssembly()
using FluentValidation;             // Thư viện validation (kiểm tra dữ liệu)
using MediatR;                      // Thư viện CQRS mediator pattern
using Microsoft.Extensions.DependencyInjection; // DI container

namespace TarotNow.Application;

/*
 * "static class": class chỉ chứa static method, không thể tạo instance.
 * Dùng cho utility functions và extension methods.
 */
public static class DependencyInjection
{
    /// <summary>
    /// Đăng ký tất cả các Application Services vào DI container.
    /// Giúp Program.cs gọn gàng hơn (Clean Architecture).
    ///
    /// Trả về IServiceCollection để hỗ trợ "method chaining":
    ///   services.AddApplicationServices().AddInfrastructureServices(config);
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        /*
         * AddMediatR: Đăng ký MediatR cho CQRS pattern.
         *
         * RegisterServicesFromAssembly: TỰ ĐỘNG scan assembly hiện tại
         * để tìm và đăng ký TẤT CẢ:
         *   - IRequestHandler<TRequest, TResponse> (command/query handlers)
         *   - INotificationHandler<TNotification> (event handlers)
         *
         * Assembly.GetExecutingAssembly(): assembly chứa file này
         *   = TarotNow.Application.dll
         *   Tất cả handler trong dll này đều được đăng ký tự động.
         *
         * AddBehavior: Đăng ký Pipeline Behavior (bước trung gian).
         *   typeof(IPipelineBehavior<,>): interface generic mở.
         *   typeof(ValidationBehavior<,>): implementation generic mở.
         *   Mọi request đều đi qua ValidationBehavior TRƯỚC handler.
         */
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            // Thêm Pipeline Behavior: Validation chạy trước mọi handler
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));
        });
        
        /*
         * AddValidatorsFromAssembly: TỰ ĐỘNG scan và đăng ký tất cả
         * FluentValidation validators trong assembly.
         *
         * Mỗi class kế thừa AbstractValidator<T> đều được đăng ký:
         *   LoginCommandValidator → IValidator<LoginCommand>
         *   RegisterCommandValidator → IValidator<RegisterCommand>
         *   ...
         *
         * Kết hợp với ValidationBehavior:
         *   Request vào → ValidationBehavior tìm IValidator cho request
         *   → chạy validation → nếu lỗi throw → nếu OK cho qua handler.
         */
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services; // Trả về để hỗ trợ method chaining
    }
}
