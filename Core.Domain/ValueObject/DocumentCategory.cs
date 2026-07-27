namespace RAGKnowledgeBase.Core.Domain.ValueObjects;

public class DocumentCategory
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DocumentCategory()
    {
    }

    public DocumentCategory(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }
}
