namespace RAGKnowledgeBase.Core.Domain.ValueObjects;

public class DocumentTag
{
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }

    public DocumentTag()
    {
    }

    public DocumentTag(string name, string? color = null)
    {
        Name = name;
        Color = color;
    }
}
