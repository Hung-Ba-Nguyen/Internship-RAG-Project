using System;

namespace RAGKnowledgeBase.Core.Application.Auth.DTOs;

public record LoginRequest(string Email, string Password);
