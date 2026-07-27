using System;
using RAGKnowledgeBase.Core.Domain.Entities.Identity;

namespace RAGKnowledgeBase.Infrastructure.Identity.Mappings;

public static class UserMappings
{
    public static User ToDomain(this ApplicationUser appUser)
    {
        if (appUser == null) return null!;
        return new User(appUser.Id, appUser.UserName ?? string.Empty, appUser.Email, appUser.FullName ?? string.Empty, appUser.IsActive);
    }

    public static ApplicationUser ToApplicationUser(this User domainUser)
    {
        if (domainUser == null) return null!;

        var app = new ApplicationUser
        {
            Id = domainUser.Id,
            UserName = domainUser.UserName,
            NormalizedUserName = domainUser.UserName?.ToUpperInvariant(),
            Email = domainUser.Email,
            NormalizedEmail = domainUser.Email?.ToUpperInvariant(),
            FullName = domainUser.FullName,
            IsActive = domainUser.IsActive,
        };

        return app;
    }
}
