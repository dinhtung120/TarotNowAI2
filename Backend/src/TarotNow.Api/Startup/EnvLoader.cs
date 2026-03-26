namespace TarotNow.Api.Startup;

public static class EnvLoader
{
    public static void Load()
    {
        var envFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".env");
        var resolvedEnvPath = Path.GetFullPath(envFilePath);

        if (File.Exists(resolvedEnvPath))
        {
            DotNetEnv.Env.Load(resolvedEnvPath);
            return;
        }

        var projectEnvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
        var resolvedProjectPath = Path.GetFullPath(projectEnvPath);
        if (File.Exists(resolvedProjectPath))
        {
            DotNetEnv.Env.Load(resolvedProjectPath);
        }
    }
}
