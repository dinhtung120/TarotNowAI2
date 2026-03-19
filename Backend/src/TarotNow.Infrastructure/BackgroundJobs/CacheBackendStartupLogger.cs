/*
 * ===================================================================
 * FILE: CacheBackendStartupLogger.cs
 * NAMESPACE: TarotNow.Infrastructure.BackgroundJobs
 * ===================================================================
 * MỤC ĐÍCH:
 *   Một Background Service Nhỏ Lúc Khởi Động App ASP.NET Lên.
 *   Chạy 1 Lần Duy Nhất Nhằm Báo Ghi Log Console Rằng Mình Đang Xài Redis Thật Hay Cùi Memory Cache Đồ Chơi Oạc Mạng (Cho Quản Trị Hệ Thống Nhanh Xử Lý).
 * ===================================================================
 */

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Hosted Service Lúc Khởi Tạo Khối Nền Application IHostedService.
/// Canh Chừng 1 Giây Bật App Bắn Ra Gạch Console Báo Cáo Hạ Tầng Chống Trục Trặc Quota Nếu Redis Kẹt.
/// </summary>
public sealed class CacheBackendStartupLogger : IHostedService
{
    private readonly CacheBackendState _cacheBackendState;
    private readonly ILogger<CacheBackendStartupLogger> _logger;

    /// <summary>Kéo Rút Thùng Đồ Chơi Từ DI Lên Chứa Tool Xem Mạng Ngon Không.</summary>
    public CacheBackendStartupLogger(
        CacheBackendState cacheBackendState,
        ILogger<CacheBackendStartupLogger> logger)
    {
        _cacheBackendState = cacheBackendState;
        _logger = logger;
    }

    /// <summary>Bật Công Tắc Nháy 1 Lần Nổ Chữ Trên Màn Hình Docker Log File.</summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_cacheBackendState.UsesRedis)
        {
            // Bền Chặt Ngon Ăn Khỏi Phải Check Nút Nghẹt Giật.
            _logger.LogInformation("Cache backend initialized with Redis.");
        }
        else
        {
            // Còi Báo Án Cảnh Nhắc Nữ Vương Admin Vào Fix Cốc Redis Rớt Mạng Lẽo Memory (Sợ 2 Máy Cục Load Balancer Chạy Sẽ Xung Đột Đếm Tiền Tách Nát Ai Quota Do Éo Đồng Bộ Được Nhau).
            _logger.LogWarning("Redis unavailable at startup. Falling back to in-memory cache; distributed rate limiting/quota consistency is reduced.");
        }

        return Task.CompletedTask;
    }

    /// <summary>Hết Hạn Nhắm Mắt.</summary>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
