#nullable enable
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb;

public class DocumentDbContextFactory : IDesignTimeDbContextFactory<DocumentDbContext>
{
    public DocumentDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString =
            configuration.GetConnectionString("DocumentDb");

        var optionsBuilder =
            new DbContextOptionsBuilder<DocumentDbContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new DocumentDbContext(optionsBuilder.Options);
    }
}
