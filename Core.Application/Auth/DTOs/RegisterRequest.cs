using System;

namespace RAGKnowledgeBase.Core.Application.Auth.DTOs;

public record RegisterRequest(string Email, string Password, string FullName);
