using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RAGKnowledgeBase.Infrastructure.Identity.Services;
using RAGKnowledgeBase.Core.Application.Auth.Interfaces;
using Xunit;

namespace Infrastructure.Identity.Tests;

public class JwtTokenGeneratorTests
{
    private JwtSettings CreateSettings()
    {
        return new JwtSettings
        {
            Secret = "ThisIsASuperSecretKeyAtLeast32Chars!",
            Issuer = "RAGKnowledgeBase",
            Audience = "RAGClients",
            ExpiryMinutes = 60
        };
    }

    [Fact]
    public void GenerateToken_ShouldContainExpectedClaims()
    {
        // Arrange
        var settings = CreateSettings();
        var options = Options.Create(settings);
        IJwtTokenGenerator generator = new JwtTokenGenerator(options);

        var userId = Guid.NewGuid();
        var email = "user@example.com";
        var roles = new List<string> { "Admin", "User" };

        // Act
        var token = generator.GenerateToken(userId, email, roles);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value.Should().Be(userId.ToString());
        jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value.Should().Be(email);

        var roleClaims = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        roleClaims.Should().BeEquivalentTo(roles);
    }

    [Fact]
    public void GenerateToken_ShouldBeValidSignatureAndIssuerAudience()
    {
        // Arrange
        var settings = CreateSettings();
        var options = Options.Create(settings);
        IJwtTokenGenerator generator = new JwtTokenGenerator(options);

        var userId = Guid.NewGuid();
        var email = "user@example.com";
        var roles = new List<string> { "User" };

        // Act
        var token = generator.GenerateToken(userId, email, roles);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,
            ValidateAudience = true,
            ValidAudience = settings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var handler = new JwtSecurityTokenHandler();

        // Assert no exception thrown during validation
        Action validate = () => handler.ValidateToken(token, validationParameters, out var _);
        validate.Should().NotThrow();
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectExpiry()
    {
        // Arrange
        var settings = CreateSettings();
        settings.ExpiryMinutes = 30;
        var options = Options.Create(settings);
        IJwtTokenGenerator generator = new JwtTokenGenerator(options);

        var userId = Guid.NewGuid();
        var email = "user@example.com";
        var roles = new List<string>();

        // Act
        var token = generator.GenerateToken(userId, email, roles);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var expiry = jwt.ValidTo;
        var now = DateTime.UtcNow;

        (expiry - now).TotalMinutes.Should().BeApproximately(settings.ExpiryMinutes, 1.5);
    }
}
