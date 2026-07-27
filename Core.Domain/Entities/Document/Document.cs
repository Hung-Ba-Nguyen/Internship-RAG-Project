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

    public Document(string title, string content, string source, DocumentCategory? category = null) : this()
    {
        Title = title;
        Content = content;
        Source = source;
        Category = category;
    }

    public void AddChunk(DocumentChunk chunk)
    {
        if (chunk == null) return;
        chunk.DocumentId = Id;
        Chunks.Add(chunk);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool RemoveChunk(Guid chunkId)
    {
        var chunk = Chunks.Find(x => x.Id == chunkId);
        if (chunk == null) return false;
        Chunks.Remove(chunk);
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    public void UpdateContent(string content)
    {
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(DocumentStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCategory(DocumentCategory? category)
    {
        Category = category;
        UpdatedAt = DateTime.UtcNow;
    }
}
