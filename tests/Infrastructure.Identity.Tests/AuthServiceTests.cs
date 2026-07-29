using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using RAGKnowledgeBase.Core.Application.Auth.DTOs;
using RAGKnowledgeBase.Core.Application.Auth.Interfaces;
using RAGKnowledgeBase.Core.Application.Exceptions;
using RAGKnowledgeBase.Infrastructure.Identity;
using RAGKnowledgeBase.Infrastructure.Identity.Services;
using Xunit;

namespace Infrastructure.Identity.Tests;

public class AuthServiceTests
{
    private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        return mgr;
    }

    [Fact]
    public async Task LoginAsync_Succeeds_WhenCredentialsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var appUser = new ApplicationUser { Id = userId, Email = "test@example.com", UserName = "test@example.com", FullName = "Test User", IsActive = true };

        var userManagerMock = CreateUserManagerMock();
        userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(appUser);
        userManagerMock.Setup(m => m.CheckPasswordAsync(appUser, It.IsAny<string>())).ReturnsAsync(true);
        userManagerMock.Setup(m => m.GetRolesAsync(appUser)).ReturnsAsync(new List<string> { "User" });

        var jwtMock = new Mock<IJwtTokenGenerator>();
        jwtMock.Setup(j => j.GenerateToken(userId, appUser.Email, It.IsAny<IList<string>>())).Returns("TOKEN123");

        var service = new AuthService(userManagerMock.Object, jwtMock.Object);

        var req = new LoginRequest(appUser.Email, "Password!23");

        // Act
        var res = await service.LoginAsync(req);

        // Assert
        res.Should().NotBeNull();
        res.Token.Should().Be("TOKEN123");
        res.Email.Should().Be(appUser.Email);
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorized_WhenInvalidCredentials()
    {
        // Arrange
        var userManagerMock = CreateUserManagerMock();
        userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

        var jwtMock = new Mock<IJwtTokenGenerator>();
        var service = new AuthService(userManagerMock.Object, jwtMock.Object);

        var req = new LoginRequest("notfound@example.com", "pwd");

        // Act
        Func<Task> act = async () => await service.LoginAsync(req);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task RegisterAsync_Succeeds_WhenNewUser()
    {
        // Arrange
        var userManagerMock = CreateUserManagerMock();
        userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);
        userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var jwtMock = new Mock<IJwtTokenGenerator>();
        jwtMock.Setup(j => j.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>())).Returns("NEW_TOKEN");

        var service = new AuthService(userManagerMock.Object, jwtMock.Object);

        var req = new RegisterRequest("new@example.com", "Password!23", "New User");

        // Act
        var res = await service.RegisterAsync(req);

        // Assert
        res.Should().NotBeNull();
        res.Token.Should().Be("NEW_TOKEN");
        res.Email.Should().Be("new@example.com");
    }

    [Fact]
    public async Task RegisterAsync_ThrowsBadRequest_WhenUserExists()
    {
        // Arrange
        var existing = new ApplicationUser { Id = Guid.NewGuid(), Email = "exists@example.com" };
        var userManagerMock = CreateUserManagerMock();
        userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(existing);

        var jwtMock = new Mock<IJwtTokenGenerator>();
        var service = new AuthService(userManagerMock.Object, jwtMock.Object);

        var req = new RegisterRequest("exists@example.com", "Password!23", "Existing");

        // Act
        Func<Task> act = async () => await service.RegisterAsync(req);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }
}
