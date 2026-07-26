#nullable enable
using System;
using RAGKnowledgeBase.Core.Domain.BuildingBlocks;

namespace RAGKnowledgeBase.Core.Domain.Entities.Auth;

public class User : Entity<Guid>, IAggregateRoot
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}