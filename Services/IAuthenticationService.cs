using NotesServer.Models;

namespace NotesServer.Services;

public interface IAuthenticationService
{
    Task<LoginResponse?> LoginAsync(string email, string password);
    string GenerateToken(User user);
}
