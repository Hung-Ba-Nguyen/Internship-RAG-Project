#nullable enable
using Microsoft.EntityFrameworkCore;
using RAGKnowledgeBase.Core.Domain.Entities.Document;
using RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb.Configurations;

namespace RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb;

public class DocumentDbContext : DbContext
{
    public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options)
    {
    }

    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentChunkConfiguration());
    }
}
