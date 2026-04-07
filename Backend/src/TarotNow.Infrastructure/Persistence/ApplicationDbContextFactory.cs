

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace TarotNow.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        
        
        var currentDirectory = Directory.GetCurrentDirectory();
        var apiProjectPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", "TarotNow.Api"));

        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        
        var connectionString = configuration.GetConnectionString("PostgreSQL")
            ?? configuration["ConnectionStrings:PostgreSQL"]
            ?? "Host=localhost;Port=5432;Database=tarotnow;Username=postgres;Password=postgres";

        
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString, options =>
            options.MigrationsHistoryTable("__ef_migrations_history"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
