using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RAGKnowledgeBase.Core.Domain.Entities.Document;
using RAGKnowledgeBase.Core.Domain.ValueObjects;
using RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb;

namespace RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb.Repositories;

public class DocumentRepository : RAGKnowledgeBase.Core.Application.Document.Repositories.IDocumentRepository
{
    private readonly DocumentDbContext _db;

    private IQueryable<Document> NonDeletedDocuments => _db.Documents.Where(d => !d.IsDeleted);

    public DocumentRepository(DocumentDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Document document)
    {
        await _db.Documents.AddAsync(document);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var doc = await _db.Documents.FindAsync(id);
        if (doc == null) return;

        doc.IsDeleted = true;
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await NonDeletedDocuments.AnyAsync(d => d.Id == id);
    }

    public async Task<Document?> GetByIdAsync(Guid id)
    {
        return await NonDeletedDocuments
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Document?> GetWithChunksAsync(Guid id)
    {
        return await NonDeletedDocuments
            .Include(d => d.Chunks)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Document>> ListAsync()
    {
        return await NonDeletedDocuments
            .ToListAsync();
    }

    public async Task<IEnumerable<Document>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return await ListAsync();
        }

        return await NonDeletedDocuments
            .Where(d => d.Title.Contains(keyword) || d.Content.Contains(keyword) || d.Source.Contains(keyword))
            .ToListAsync();
    }

    public async Task<IEnumerable<Document>> GetByStatusAsync(DocumentStatus status)
    {
        return await NonDeletedDocuments
            .Where(d => d.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<DocumentChunk>> GetChunksAsync(Guid documentId)
    {
        return await _db.DocumentChunks
            .Include(c => c.Document)
            .Where(c => c.DocumentId == documentId && c.Document != null && !c.Document.IsDeleted)
            .ToListAsync();
    }

    public async Task UpdateAsync(Document document)
    {
        _db.Documents.Update(document);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Document>> FindByCategoryAsync(string categoryName)
    {
        return await NonDeletedDocuments
            .Where(d => d.Category != null && d.Category.Name == categoryName)
            .ToListAsync();
    }
}
