using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RAGKnowledgeBase.Infrastructure.Identity;

namespace RAGKnowledgeBase.Infrastructure.Identity;

public class AuthDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Rename Identity tables to concise names
        builder.Entity<ApplicationUser>(b =>
        {
            b.ToTable("Users");
            b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("IX_Users_NormalizedUserName");
            b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("IX_Users_NormalizedEmail");
        });

        builder.Entity<IdentityRole<Guid>>(b =>
        {
            b.ToTable("Roles");
            b.HasIndex(r => r.NormalizedName).HasDatabaseName("IX_Roles_NormalizedName");
        });

        builder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("UserRoles");
            b.HasIndex(ur => ur.UserId).HasDatabaseName("IX_UserRoles_UserId");
            b.HasIndex(ur => ur.RoleId).HasDatabaseName("IX_UserRoles_RoleId");
        });

        builder.Entity<IdentityUserClaim<Guid>>(b => b.ToTable("UserClaims"));
        builder.Entity<IdentityRoleClaim<Guid>>(b => b.ToTable("RoleClaims"));
        builder.Entity<IdentityUserLogin<Guid>>(b => b.ToTable("UserLogins"));
        builder.Entity<IdentityUserToken<Guid>>(b => b.ToTable("UserTokens"));

        // Add indexes for frequently queried FK fields
        builder.Entity<IdentityUserClaim<Guid>>().HasIndex(uc => uc.UserId).HasDatabaseName("IX_UserClaims_UserId");
        builder.Entity<IdentityRoleClaim<Guid>>().HasIndex(rc => rc.RoleId).HasDatabaseName("IX_RoleClaims_RoleId");
        builder.Entity<IdentityUserLogin<Guid>>().HasIndex(l => l.UserId).HasDatabaseName("IX_UserLogins_UserId");
        builder.Entity<IdentityUserToken<Guid>>().HasIndex(t => t.UserId).HasDatabaseName("IX_UserTokens_UserId");
    }
}
