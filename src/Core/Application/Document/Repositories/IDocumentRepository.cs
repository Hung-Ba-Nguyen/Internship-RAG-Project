using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RAGKnowledgeBase.Core.Domain.Entities.Document;
using RAGKnowledgeBase.Core.Domain.ValueObjects;

namespace RAGKnowledgeBase.Core.Application.Document.Repositories;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(Guid id);
    Task<Document?> GetWithChunksAsync(Guid id);
    Task<IEnumerable<Document>> ListAsync();
    Task<IEnumerable<Document>> SearchAsync(string keyword);
    Task<IEnumerable<Document>> GetByStatusAsync(DocumentStatus status);
    Task<bool> ExistsAsync(Guid id);
    Task AddAsync(Document document);
    Task UpdateAsync(Document document);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Document>> FindByCategoryAsync(string categoryName);
    Task<IEnumerable<DocumentChunk>> GetChunksAsync(Guid documentId);
}
