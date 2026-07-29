using System.Threading.Tasks;
using RAGKnowledgeBase.Core.Application.Auth.DTOs;

namespace RAGKnowledgeBase.Core.Application.Auth.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
}
