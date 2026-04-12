using NeoSolve.ImageSharp.AVIF;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Đăng ký AVIF encoder/decoder NeoSolve một lần cho ImageSharp.
/// </summary>
public static class ImageSharpAvifInitializer
{
    private static readonly object Gate = new();
    private static bool _configured;

    public static void EnsureConfigured()
    {
        lock (Gate)
        {
            if (_configured)
            {
                return;
            }

            global::SixLabors.ImageSharp.Configuration.Default.Configure(new AVIFConfigurationModule());
            _configured = true;
        }
    }
}
