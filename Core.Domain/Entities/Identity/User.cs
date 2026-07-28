using System;
using RAGKnowledgeBase.Core.Domain.BuildingBlocks;

namespace RAGKnowledgeBase.Core.Domain.Entities.Identity;

public class User : Entity<Guid>, IAggregateRoot
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public User()
    {
        Id = Guid.NewGuid();
    }

    public User(string userName, string? email, string fullName) : this()
    {
        UserName = userName;
        Email = email;
        FullName = fullName;
    }

    public User(Guid id, string userName, string? email, string fullName, bool isActive)
    {
        Id = id;
        UserName = userName;
        Email = email;
        FullName = fullName;
        IsActive = isActive;
    }
}
