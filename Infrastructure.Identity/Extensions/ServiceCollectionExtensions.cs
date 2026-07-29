using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RAGKnowledgeBase.Infrastructure.Identity;
using RAGKnowledgeBase.Infrastructure.Identity.Services;
using RAGKnowledgeBase.Core.Application.Auth.Interfaces;

namespace RAGKnowledgeBase.Infrastructure.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var connectionString = configuration.GetConnectionString("AuthConnection")
                               ?? configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'AuthConnection' or 'DefaultConnection' not found. Ensure appsettings.json contains the connection string.");
        }

        services.AddDbContext<AuthDbContext>(opts => opts.UseSqlServer(connectionString));

        // Configure JwtSettings and register JWT token generator
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddSingleton<RAGKnowledgeBase.Core.Application.Auth.Interfaces.IJwtTokenGenerator, RAGKnowledgeBase.Infrastructure.Identity.Services.JwtTokenGenerator>();

        // Manually build Identity chain to avoid relying on AddIdentity extension resolution
        var identityBuilder = new Microsoft.AspNetCore.Identity.IdentityBuilder(typeof(ApplicationUser), typeof(IdentityRole<Guid>), services);
        identityBuilder.AddEntityFrameworkStores<AuthDbContext>();
        // Register default token providers (email/phone/password reset, etc.)
        identityBuilder.AddDefaultTokenProviders();

        // Register AuthService
        services.AddScoped<RAGKnowledgeBase.Core.Application.Auth.Interfaces.IAuthService, RAGKnowledgeBase.Infrastructure.Identity.Services.AuthService>();

        return services;
    }
}
