using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RAGKnowledgeBase.Core.Domain.ValueObjects;
using RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb;

namespace RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb.Repositories;

public class CategoryRepository : RAGKnowledgeBase.Core.Application.Document.Repositories.ICategoryRepository
{
    private readonly DocumentDbContext _db;

    public CategoryRepository(DocumentDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<DocumentCategory>> ListAsync()
    {
        return await _db.Documents
            .Where(d => !d.IsDeleted && d.Category != null)
            .Select(d => d.Category!)
            .Distinct()
            .ToListAsync();
    }

    public async Task<DocumentCategory?> GetByNameAsync(string name)
    {
        return await _db.Documents
            .Where(d => !d.IsDeleted && d.Category != null && d.Category!.Name == name)
            .Select(d => d.Category!)
            .FirstOrDefaultAsync();
    }
}
