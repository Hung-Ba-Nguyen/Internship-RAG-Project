using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RAGKnowledgeBase.Infrastructure.Identity;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        // Build configuration from appsettings.json (searching up from the current directory)
        var basePath = Directory.GetCurrentDirectory();
        var configuration = BuildConfiguration(basePath);

        var connectionString = configuration.GetConnectionString("AuthConnection")
                               ?? configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'AuthConnection' or 'DefaultConnection' not found. Ensure appsettings.json contains the connection string.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AuthDbContext(optionsBuilder.Options);
    }

    private static IConfiguration BuildConfiguration(string startingDirectory)
    {
        // Walk up directories until we find an appsettings.json
        var dir = new DirectoryInfo(startingDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "appsettings.json")))
        {
            dir = dir.Parent;
        }

        var configBuilder = new ConfigurationBuilder();
        var configBase = dir != null ? dir.FullName : startingDirectory;
        configBuilder.SetBasePath(configBase)
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                     .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: false)
                     .AddEnvironmentVariables();

        return configBuilder.Build();
    }
}
