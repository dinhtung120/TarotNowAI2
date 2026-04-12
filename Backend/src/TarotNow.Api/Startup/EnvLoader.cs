using DotNetEnv;

namespace TarotNow.Api.Startup;

/// <summary>
/// Nạp <c>.env</c> ở root repository (cùng file mà Docker Compose tự đọc) và ánh xạ tên biến kiểu compose
/// sang biến môi trường ASP.NET Core (<c>__</c>) khi chạy <c>dotnet run</c> cục bộ.
/// </summary>
public static class EnvLoader
{
    public static void Load()
    {
        var path = ResolveRepoDotEnvPath();
        if (path is not null)
        {
            // Không ghi đè biến đã có (integration test / CI / shell) để tránh lệch so với Testcontainers.
            Env.Load(path, new LoadOptions(clobberExistingVars: false));
        }

        ApplyDockerComposeStyleEnvironmentAliases();
        EnsureLocalConnectionStringsFromDatabaseEnvVars();
    }

    private static string? ResolveRepoDotEnvPath()
    {
        foreach (var start in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
        {
            if (string.IsNullOrWhiteSpace(start)) continue;

            var dir = new DirectoryInfo(start);
            while (dir is not null)
            {
                var candidate = Path.Combine(dir.FullName, ".env");
                if (File.Exists(candidate)) return candidate;

                dir = dir.Parent;
            }
        }

        return null;
    }

    private static void ApplyDockerComposeStyleEnvironmentAliases()
    {
        CopyEnvIfTargetMissing("JWT_SECRETKEY", "JWT__SECRETKEY");
        CopyEnvIfTargetMissing("JWT_ISSUER", "JWT__ISSUER");
        CopyEnvIfTargetMissing("JWT_AUDIENCE", "JWT__AUDIENCE");
        CopyEnvIfTargetMissing("JWT_EXPIRYMINUTES", "JWT__EXPIRYMINUTES");
        CopyEnvIfTargetMissing("JWT_REFRESHEXPIRYDAYS", "JWT__REFRESHEXPIRYDAYS");

        CopyEnvIfTargetMissing("PAYMENT_WEBHOOK_SECRET", "PAYMENTGATEWAY__WEBHOOKSECRET");
        CopyEnvIfTargetMissing("MFA_ENCRYPTION_KEY", "SECURITY__MFAENCRYPTIONKEY");

        CopyEnvIfTargetMissing("SMTP_HOST", "SMTP__HOST");
        CopyEnvIfTargetMissing("SMTP_PORT", "SMTP__PORT");
        CopyEnvIfTargetMissing("SMTP_USERNAME", "SMTP__USERNAME");
        CopyEnvIfTargetMissing("SMTP_PASSWORD", "SMTP__PASSWORD");
        CopyEnvIfTargetMissing("SMTP_SENDER_EMAIL", "SMTP__SENDEREMAIL");
        CopyEnvIfTargetMissing("SMTP_SENDER_NAME", "SMTP__SENDERNAME");

        CopyEnvIfTargetMissing("AI_API_KEY", "AIPROVIDER__APIKEY");
        CopyEnvIfTargetMissing("AI_BASE_URL", "AIPROVIDER__BASEURL");
        CopyEnvIfTargetMissing("AI_MODEL", "AIPROVIDER__MODEL");
        CopyEnvIfTargetMissing("AI_TIMEOUT_SECONDS", "AIPROVIDER__TIMEOUTSECONDS");
        CopyEnvIfTargetMissing("AI_MAX_RETRIES", "AIPROVIDER__MAXRETRIES");

        CopyEnvIfTargetMissing("PUBLIC_BASE_URL", "CORS__ALLOWEDORIGINS__0");
        CopyEnvIfTargetMissing("NEXT_PUBLIC_BASE_URL", "CORS__ALLOWEDORIGINS__0");

        CopyEnvIfTargetMissing("REDIS_INSTANCE_NAME", "REDIS__INSTANCENAME");

        CopyEnvIfTargetMissing("OBJECTSTORAGE_PROVIDER", "OBJECTSTORAGE__PROVIDER");
        CopyEnvIfTargetMissing("OBJECTSTORAGE_MAX_CONCURRENT_UPLOADS", "OBJECTSTORAGE__MAXCONCURRENTUPLOADS");
        CopyEnvIfTargetMissing("OBJECTSTORAGE_MAX_UPLOAD_BYTES", "OBJECTSTORAGE__MAXUPLOADBYTES");
        CopyEnvIfTargetMissing("OBJECTSTORAGE_AVATAR_MAX_EDGE", "OBJECTSTORAGE__AVATARMAXEDGEPIXELS");
        CopyEnvIfTargetMissing("OBJECTSTORAGE_COMMUNITY_MAX_EDGE", "OBJECTSTORAGE__COMMUNITYIMAGEMAXEDGEPIXELS");

        CopyEnvIfTargetMissing("R2_ACCOUNT_ID", "OBJECTSTORAGE__R2__ACCOUNTID");
        CopyEnvIfTargetMissing("R2_ACCESS_KEY_ID", "OBJECTSTORAGE__R2__ACCESSKEYID");
        CopyEnvIfTargetMissing("R2_SECRET_ACCESS_KEY", "OBJECTSTORAGE__R2__SECRETACCESSKEY");
        CopyEnvIfTargetMissing("R2_BUCKET_NAME", "OBJECTSTORAGE__R2__BUCKETNAME");
        CopyEnvIfTargetMissing("R2_PUBLIC_BASE_URL", "OBJECTSTORAGE__R2__PUBLICBASEURL");

        CopyEnvIfTargetMissing("MEDIACDN_PUBLIC_BASE_URL", "MEDIACDN__PUBLICBASEURL");

        CopyEnvIfTargetMissing("FORWARDED_NETWORK_0", "FORWARDEDHEADERS__KNOWNNETWORKS__0");
    }

    /// <summary>
    /// Khi chạy API ngoài Docker, compose không inject CONNECTIONSTRINGS — tạo từ POSTGRES_*/MONGO_DB/REDIS_PORT nếu thiếu.
    /// </summary>
    private static void EnsureLocalConnectionStringsFromDatabaseEnvVars()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__POSTGRESQL")))
        {
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
            if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password) &&
                !string.IsNullOrWhiteSpace(database))
            {
                var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
                if (string.IsNullOrWhiteSpace(port)) port = "5432";

                Environment.SetEnvironmentVariable(
                    "CONNECTIONSTRINGS__POSTGRESQL",
                    $"Host=localhost;Port={port};Database={database};Username={user};Password={password}");
            }
        }

        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__MONGODB")))
        {
            var mongoDb = Environment.GetEnvironmentVariable("MONGO_DB");
            if (string.IsNullOrWhiteSpace(mongoDb)) mongoDb = "tarotweb";

            Environment.SetEnvironmentVariable(
                "CONNECTIONSTRINGS__MONGODB",
                $"mongodb://localhost:27017/{mongoDb}");
        }

        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__REDIS")))
        {
            var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");
            if (string.IsNullOrWhiteSpace(redisPort)) redisPort = "6379";

            Environment.SetEnvironmentVariable("CONNECTIONSTRINGS__REDIS", $"localhost:{redisPort}");
        }
    }

    private static void CopyEnvIfTargetMissing(string sourceName, string targetName)
    {
        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(targetName))) return;

        var value = Environment.GetEnvironmentVariable(sourceName);
        if (string.IsNullOrWhiteSpace(value)) return;

        Environment.SetEnvironmentVariable(targetName, value);
    }
}
