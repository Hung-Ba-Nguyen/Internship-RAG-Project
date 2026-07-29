using System;

namespace RAGKnowledgeBase.Core.Application.Auth.DTOs;

public record AuthResponse(string Token, string RefreshToken, Guid UserId, string Email, string FullName);
