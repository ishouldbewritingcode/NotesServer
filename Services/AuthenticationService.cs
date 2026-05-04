using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NotesServer.Models;

namespace NotesServer.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly INoteService _noteService;
    private readonly IConfiguration _configuration;

    public AuthenticationService(INoteService noteService, IConfiguration configuration)
    {
        _noteService = noteService;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        // For this sample app, we just verify the user exists by email
        // In a production app, you would hash and verify the password here
        var user = await _noteService.GetUserByEmail(email);
        if (user == null)
        {
            return null;
        }

        var token = GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            User = new UserInfo
            {
                Id = user.Id.ToString(),
                Email = user.Email
            }
        };
    }

    public string GenerateToken(User user)
    {
        var key = _configuration["Jwt:SecretKey"] 
            ?? throw new InvalidOperationException("JWT secret key not configured");
        var issuer = _configuration["Jwt:Issuer"] ?? "NotesServer";
        var audience = _configuration["Jwt:Audience"] ?? "NotesApp";
        var expiryMinutes = int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var expiry) ? expiry : 60;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
