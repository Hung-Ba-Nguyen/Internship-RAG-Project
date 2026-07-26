#nullable enable
using Microsoft.EntityFrameworkCore;
using RAGKnowledgeBase.Core.Domain.Entities.Auth;

namespace RAGKnowledgeBase.Infrastructure.Identity;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
}