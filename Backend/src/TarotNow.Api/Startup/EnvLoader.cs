namespace TarotNow.Api.Startup;

// Nạp file Backend/.env theo nhiều vị trí fallback để chạy ổn định cả trong IDE lẫn môi trường build/runtime khác nhau.
public static class EnvLoader
{
    /// <summary>
    /// Nạp biến môi trường từ file `Backend/.env` nếu tồn tại.
    /// Luồng xử lý: ưu tiên đường dẫn tương đối từ output base directory, fallback sang đường dẫn theo current directory.
    /// </summary>
    public static void Load()
    {
        var envFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".env");
        var resolvedEnvPath = Path.GetFullPath(envFilePath);

        if (File.Exists(resolvedEnvPath))
        {
            // Nhánh ưu tiên: chạy từ output folder (bin) vẫn tìm đúng Backend/.env.
            DotNetEnv.Env.Load(resolvedEnvPath);
            return;
        }

        var projectEnvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
        var resolvedProjectPath = Path.GetFullPath(projectEnvPath);
        if (File.Exists(resolvedProjectPath))
        {
            // Fallback cho ngữ cảnh chạy trực tiếp từ project directory.
            DotNetEnv.Env.Load(resolvedProjectPath);
        }
    }
}
