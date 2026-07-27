#nullable enable
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb;

public class DocumentDbContextFactory : IDesignTimeDbContextFactory<DocumentDbContext>
{
    public DocumentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DocumentDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=DocumentDb;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new DocumentDbContext(optionsBuilder.Options);
    }
}
