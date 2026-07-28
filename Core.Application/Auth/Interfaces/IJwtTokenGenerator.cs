using System;
using System.Collections.Generic;

namespace RAGKnowledgeBase.Core.Application.Auth.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, IList<string> roles);
}
