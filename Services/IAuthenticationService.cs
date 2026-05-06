using NotesServer.Models;

namespace NotesServer.Services;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginAsync(string email, string? name = null);
    string GenerateToken(User user);
}
