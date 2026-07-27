using System;
using System.Collections.Generic;
using RAGKnowledgeBase.Core.Domain.BuildingBlocks;
using RAGKnowledgeBase.Core.Domain.ValueObjects;

namespace RAGKnowledgeBase.Core.Domain.Entities.Document;

public class Document : Entity<Guid>, IAggregateRoot
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public string? MimeType { get; set; }
    public long SizeInBytes { get; set; }
    public DocumentCategory? Category { get; set; }
    public List<DocumentTag> Tags { get; set; } = new();
    public DocumentStatus Status { get; set; } = DocumentStatus.Uploading;
    public string Source { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public List<DocumentChunk> Chunks { get; set; } = new();

    public Document()
    {
        Id = Guid.NewGuid();
    }

    public Document(string title, string content, string source) : this()
    {
        Title = title;
        Content = content;
        Source = source;
    }
}
