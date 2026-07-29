using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RAGKnowledgeBase.Core.Domain.Entities.Document;
using RAGKnowledgeBase.Core.Domain.ValueObjects;

namespace RAGKnowledgeBase.Core.Application.Document.Repositories;

public interface IDocumentRepository
{
    Task<RAGKnowledgeBase.Core.Domain.Entities.Document.Document?> GetByIdAsync(Guid id);
    Task<RAGKnowledgeBase.Core.Domain.Entities.Document.Document?> GetWithChunksAsync(Guid id);
    Task<IEnumerable<RAGKnowledgeBase.Core.Domain.Entities.Document.Document>> ListAsync();
    Task<IEnumerable<RAGKnowledgeBase.Core.Domain.Entities.Document.Document>> SearchAsync(string keyword);
    Task<IEnumerable<RAGKnowledgeBase.Core.Domain.Entities.Document.Document>> GetByStatusAsync(DocumentStatus status);
    Task<bool> ExistsAsync(Guid id);
    Task AddAsync(RAGKnowledgeBase.Core.Domain.Entities.Document.Document document);
    Task UpdateAsync(RAGKnowledgeBase.Core.Domain.Entities.Document.Document document);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<RAGKnowledgeBase.Core.Domain.Entities.Document.Document>> FindByCategoryAsync(string categoryName);
    Task<IEnumerable<RAGKnowledgeBase.Core.Domain.Entities.Document.DocumentChunk>> GetChunksAsync(Guid documentId);
}
