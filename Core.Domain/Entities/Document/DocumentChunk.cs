#nullable enable
using System;
using RAGKnowledgeBase.Core.Domain.BuildingBlocks;

namespace RAGKnowledgeBase.Core.Domain.Entities.Document;

public class DocumentChunk : Entity<Guid>
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = string.Empty;
    public int TokenCount { get; set; }
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DocumentChunk()
    {
        Id = Guid.NewGuid();
    }

    public DocumentChunk(Guid documentId, int chunkIndex, string content) : this()
    {
        DocumentId = documentId;
        ChunkIndex = chunkIndex;
        Content = content;
        TokenCount = 0;
    }
}
