using System.Collections.Generic;
using System.Threading.Tasks;
using RAGKnowledgeBase.Core.Domain.ValueObjects;

namespace RAGKnowledgeBase.Core.Application.Document.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<DocumentCategory>> ListAsync();
    Task<DocumentCategory?> GetByNameAsync(string name);
}
