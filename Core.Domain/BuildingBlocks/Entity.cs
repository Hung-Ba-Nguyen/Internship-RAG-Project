#nullable enable
using System;

namespace RAGKnowledgeBase.Core.Domain.BuildingBlocks;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;
}