#nullable enable
using System;
using RAGKnowledgeBase.Core.Domain.BuildingBlocks;

namespace RAGKnowledgeBase.Core.Domain.Entities.Auth;

public class Role : Entity<Guid>, IAggregateRoot
{
    public string Name { get; set; } = string.Empty;
}